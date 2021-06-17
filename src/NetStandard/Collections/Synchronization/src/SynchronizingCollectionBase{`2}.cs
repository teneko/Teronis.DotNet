// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.Synchronization.PostConfigurators;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// A synchronizing collection.
    /// <br/>Bindable collections are <see cref="SubItems"/> and <see cref="SuperItems"/>.
    /// <br/>Remember: All childs are parents (because child extends parent), so all childs are a subset of parent / parent is a superset of child.
    /// </summary>
    /// <typeparam name="TSubItem"></typeparam>
    /// <typeparam name="TSuperItem"></typeparam>
    public abstract partial class SynchronizingCollectionBase<TSuperItem, TSubItem> : INotifyCollectionModification<TSuperItem, TSubItem>, ICollectionSynchronizationContext<TSuperItem>
        where TSuperItem : notnull
        where TSubItem : notnull
    {
        private static CollectionModification<TSuperItem, TSubItem> replaceOldSuperItemsByOldSubItems(
            ICollectionModification<TSuperItem, TSuperItem> superItemsSuperItemsModification,
            IReadOnlyCollection<TSubItem> subItems)
        {
            var oldItems = superItemsSuperItemsModification.GetItemsBeginningFromOldIndex(subItems);
            var subItemsSuperItemsModification = superItemsSuperItemsModification.CopyWithOtherItems(oldItems, superItemsSuperItemsModification.NewItems);
            return subItemsSuperItemsModification;
        }

        public event EventHandler? CollectionSynchronizing;
        public event EventHandler? CollectionSynchronized;
        public event NotifyCollectionModifiedEventHandler<TSuperItem, TSubItem>? CollectionModified;

        public ICollectionSynchronizationMethod<TSuperItem, TSuperItem> SynchronizationMethod { get; }

        public ISynchronizedCollection<TSuperItem> SuperItems =>
            superItems;

        public ISynchronizedCollection<TSubItem> SubItems =>
            subItems;

        protected ICollectionChangeHandler<TSuperItem> SuperItemChangeHandler =>
            superItemChangeHandler;

        protected ICollectionChangeHandler<TSubItem> SubItemChangeHandler =>
            subItemChangeHandler;

        private CollectionUpdateItemDelegate<TSuperItem, TSubItem>? superItemUpdateHandler;
        private CollectionUpdateItemDelegate<TSubItem, TSuperItem>? subItemUpdateHandler;

        private ICollectionChangeHandler<TSuperItem> superItemChangeHandler;
        private ICollectionChangeHandler<TSubItem> subItemChangeHandler;

        private ISynchronizedCollection<TSuperItem> superItems;
        private ISynchronizedCollection<TSubItem> subItems;

        public SynchronizingCollectionBase(SynchronizingCollectionOptions<TSuperItem, TSubItem>? options)
        {
            /* Initialize collections. */
            options = options ?? new SynchronizingCollectionOptions<TSuperItem, TSubItem>();
            ConfigureItems(options);

            SynchronizingCollectionOptionsPostConfigurator.Default.PostConfigure(
                options.SuperItemsOptions,
                out superItemChangeHandler,
                items => new SuperItemCollection(items, options.SuperItemsOptions, this),
                out superItems);

            SynchronizingCollectionOptionsPostConfigurator.Default.PostConfigure(
                options.SubItemsOptions,
                out subItemChangeHandler,
                items => new SubItemCollection(items, options.SubItemsOptions, this),
                out subItems);

            superItemUpdateHandler = options.SuperItemsOptions.ItemUpdateHandler;
            subItemUpdateHandler = options.SubItemsOptions.ItemUpdateHandler;

            SynchronizationMethod = options.SynchronizationMethod
                ?? CollectionSynchronizationMethod.Sequential<TSuperItem>();
        }

        public SynchronizingCollectionBase()
            : this(options: null) { }

        private void InvokeCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        private void InvokeCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        /// <summary>
        /// Configures items options.
        /// </summary>
        /// <param name="options">The options that got passed to constructor or a new instance.</param>
        protected virtual void ConfigureItems(SynchronizingCollectionOptions<TSuperItem, TSubItem> options)
        { }

        protected abstract TSubItem CreateSubItem(TSuperItem superItem);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="collectionToBeMirrored"/> are 
        /// forwarded to <see cref="ProcessModification(ICollectionModification{TSuperItem, TSuperItem})"/>
        /// of this instance.
        /// </summary>
        /// <param name="collectionToBeMirrored">The foreign collection that is about to be mirrored related to its modifications.</param>
        /// <returns>A collection synchronization mirror.</returns>
        public SynchronizedCollectionMirror<TSuperItem> MirrorSynchronizedCollection(ISynchronizedCollection<TSuperItem> collectionToBeMirrored) =>
            new SynchronizedCollectionMirror<TSuperItem>(this, collectionToBeMirrored);

        protected virtual void OnAfterAddItem(int addedItemIndex) { }

        protected virtual void AddItems(ApplyingCollectionModifications applyingModifications)
        {
            var subSuperItemModification = applyingModifications.SuperSubItemModification;
            // ! because we use it at null checked location because it gets null checked before.
            var subSuperItemModifiactionNewItems = subSuperItemModification.NewItems!;

            TSuperItem superItem = default!;

            CollectionModificationIterationTools.BeginInsert(subSuperItemModification)
                // subSuperItemModifiactionNewItems is now null checked.
                .OnIteration(iterationContext => {
                    superItem = subSuperItemModifiactionNewItems[iterationContext.ModificationItemIndex];
                    SuperItemChangeHandler.InsertItem(iterationContext.CollectionItemIndex, superItem);
                })
                .OnIteration(iterationContext => {
                    var subItem = CreateSubItem(superItem);
                    SubItemChangeHandler.InsertItem(iterationContext.CollectionItemIndex, subItem);
                })
                .OnIteration(iterationContext => OnAfterAddItem(iterationContext.CollectionItemIndex))
                .Iterate();
        }

        protected virtual void OnBeforeRemoveItem(int removingItemIndex) { }

        protected virtual void RemoveItems(ApplyingCollectionModifications applyingModifications)
        {
            var superItemModification = applyingModifications.SuperItemModification;

            CollectionModificationIterationTools.BeginRemove(superItemModification)
                .OnIteration(iterationContext => SuperItemChangeHandler.RemoveItem(iterationContext.CollectionItemIndex))
                .OnIteration(iterationContext => SubItemChangeHandler.RemoveItem(iterationContext.CollectionItemIndex))
                .Iterate();
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="applyingModifications"></param>
        protected virtual void MoveItems(ApplyingCollectionModifications applyingModifications)
        {
            var modification = applyingModifications.SuperSubItemModification;
            CollectionModificationIterationTools.CheckMove(modification);
            var oldItemsCount = modification.OldItems!.Count;
            SubItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
            SuperItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
        }

        protected virtual void OnBeforeReplaceItem(int replacedItemIndex) { }

        protected virtual void OnAfterReplaceItem(int replacedItemIndex) { }

        /// <summary>
        /// Has default implementation: Calls <see cref="SuperItemChangeHandler"/>/<see cref="SubItemChangeHandler"/>
        /// its <see cref="ICollectionChangeHandler{TItem}.ReplaceItem(int, Func{TItem})"/>
        /// method when <see cref="ICollectionChangeHandler{TItem}.CanReplaceItem"/> is true.
        /// </summary>
        /// <param name="applyingModifications"></param>
        protected virtual void ReplaceItems(ApplyingCollectionModifications applyingModifications)
        {
            var canReplaceOrUpdateSuperItem = SuperItemChangeHandler.CanReplaceItem || !(superItemUpdateHandler is null);
            var canReplaceOrUpdateSubItem = SubItemChangeHandler.CanReplaceItem || !(subItemUpdateHandler is null);

            if (canReplaceOrUpdateSuperItem || canReplaceOrUpdateSubItem) {
                var superItemModification = applyingModifications.SuperItemModification;
                var iteratorBuilder = CollectionModificationIterationTools.BeginReplace(superItemModification);
                var lazyCreatedSubItemByIndex = new Dictionary<int, SlimLazy<TSubItem>>();
                //int replaceItemIndex = 0;

                iteratorBuilder.OnIteration(iterationContext => {
                    lazyCreatedSubItemByIndex[iterationContext.ModificationItemIndex] = new SlimLazy<TSubItem>(() =>
                        CreateSubItem(superItemModification.NewItems![iterationContext.ModificationItemIndex]));
                });

                if (SuperItemChangeHandler.CanReplaceItem) {
                    iteratorBuilder.OnIteration(iterationContext => {
                        // We do not know, whether the old item gets
                        // consumed, so we provide it but lazy.
                        var lazyNewItem = new SlimLazy<TSuperItem>(() =>
                            superItemModification.NewItems![iterationContext.ModificationItemIndex]);

                        SuperItemChangeHandler.ReplaceItem(iterationContext.CollectionItemIndex, lazyNewItem.GetValue);
                    });
                }

                iteratorBuilder.OnIteration(iterationContext => OnBeforeReplaceItem(iterationContext.CollectionItemIndex));

                if (SubItemChangeHandler.CanReplaceItem) {
                    iteratorBuilder.OnIteration(iterationContext =>
                        SubItemChangeHandler.ReplaceItem(iterationContext.CollectionItemIndex, lazyCreatedSubItemByIndex[iterationContext.ModificationItemIndex].GetValue));
                }

                void UpdateItem(int modificationItemIndex, int collectionItemIndex)
                {
                    if (!(superItemUpdateHandler is null)) {
                        superItemUpdateHandler(
                            SuperItemChangeHandler.Items[collectionItemIndex],
                            // We want update with the latest super item (when used
                            // and accessible from user code) and latest sub item.
                            () => lazyCreatedSubItemByIndex[modificationItemIndex].Value);
                    }

                    if (!(subItemUpdateHandler is null)) {
                        subItemUpdateHandler(
                            SubItemChangeHandler.Items[collectionItemIndex],
                            () => superItemModification.NewItems![modificationItemIndex]);
                    }
                }

                iteratorBuilder.OnIteration(iterationContext => UpdateItem(iterationContext.ModificationItemIndex, iterationContext.CollectionItemIndex));
                iteratorBuilder.OnIteration(iterationContext => OnAfterReplaceItem(iterationContext.CollectionItemIndex));
                iteratorBuilder.Iterate();
            }
        }

        protected virtual void ResetItems(ApplyingCollectionModifications applyingModifications)
        {
            SubItemChangeHandler.Reset();
            SuperItemChangeHandler.Reset();
        }

        protected virtual void ProcessModification(ApplyingCollectionModifications applyingModifications)
        {
            switch (applyingModifications.SuperItemModification.Action) {
                case NotifyCollectionChangedAction.Add:
                    AddItems(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItems(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItems(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetItems(applyingModifications);
                    break;
            }
        }

        protected void OnCollectionModified(CollectionModifiedEventArgs<TSuperItem, TSubItem> args)
            => CollectionModified?.Invoke(this, args);

        protected void ProcessModification(ICollectionModification<TSuperItem, TSuperItem> superItemModification)
        {
            var oldSubItemsNewSuperItemsModification = replaceOldSuperItemsByOldSubItems(superItemModification, SubItems);
            var applyingModificationBundle = new ApplyingCollectionModifications(oldSubItemsNewSuperItemsModification, superItemModification);
            ProcessModification(applyingModificationBundle);

            /*  We want transform new-super-items to new-sub-items because
             *  they have been created previously if any is existing. */
            var newSubItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(SubItems);
            var oldSubItemsNewSubItemsModification = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldSubItemsNewSuperItemsModification.OldItems, newSubItems);

            var collectionModifiedArgs = new CollectionModifiedEventArgs<TSuperItem, TSubItem>(
                oldSubItemsNewSubItemsModification,
                applyingModificationBundle.SuperSubItemModification,
                applyingModificationBundle.SuperItemModification);

            OnCollectionModified(collectionModifiedArgs);
        }

        /// <summary>
        /// Synchronizes collection with <paramref name="items"/>.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="yieldCapabilities"></param>
        public virtual void SynchronizeCollection(IEnumerable<TSuperItem>? items, CollectionModificationYieldCapabilities yieldCapabilities)
        {
            var enumerator = SynchronizationMethod
                .YieldCollectionModifications(
                     SuperItemChangeHandler.Items.AsIList().ToProducedListModificationsNotBatchedMarker(),
                    items,
                    yieldCapabilities)
                .GetEnumerator();

            if (!enumerator.MoveNext()) {
                return;
            }

            InvokeCollectionSynchronizing();

            do {
                ProcessModification(enumerator.Current);
            } while (enumerator.MoveNext());

            InvokeCollectionSynchronized();
        }

        public void SynchronizeCollection(IEnumerable<TSuperItem>? enumerable) =>
            SynchronizeCollection(enumerable, yieldCapabilities: CollectionModificationYieldCapabilities.All);

        #region ICollectionSynchronizationContext<SuperItemType>

        void ICollectionSynchronizationContext<TSuperItem>.BeginCollectionSynchronization() =>
            InvokeCollectionSynchronizing();

        void ICollectionSynchronizationContext<TSuperItem>.ProcessModification(ICollectionModification<TSuperItem, TSuperItem> superItemModification) =>
            ProcessModification(superItemModification);

        void ICollectionSynchronizationContext<TSuperItem>.EndCollectionSynchronization() =>
            InvokeCollectionSynchronized();

        #endregion

        protected readonly struct ApplyingCollectionModifications
        {
            public ICollectionModification<TSuperItem, TSubItem> SuperSubItemModification { get; }
            public ICollectionModification<TSuperItem, TSuperItem> SuperItemModification { get; }

            public ApplyingCollectionModifications(
                ICollectionModification<TSuperItem, TSubItem> subSuperItemModification,
                ICollectionModification<TSuperItem, TSuperItem> superItemModification)
            {
                SuperSubItemModification = subSuperItemModification;
                SuperItemModification = superItemModification;
            }
        }
    }
}
