// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms;
using Teronis.Collections.Algorithms.Modifications;
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

        public ISynchronizedCollection<TSubItem> SubItems { get; }
        public ISynchronizedCollection<TSuperItem> SuperItems { get; }
        public ICollectionSynchronizationMethod<TSuperItem, TSuperItem> SynchronizationMethod { get; }

        protected CollectionChangeHandler<TSubItem>.IDependencyInjectedHandler SubItemChangeHandler { get; }
        protected CollectionChangeHandler<TSuperItem>.IDependencyInjectedHandler SuperItemChangeHandler { get; }

        private CollectionUpdateItemDelegate<TSuperItem, TSubItem>? updateSuperItem;
        private CollectionUpdateItemDelegate<TSubItem, TSuperItem>? updateSubItem;

        public SynchronizingCollectionBase(Options? options)
        {
            /* Initialize collections. */
            options = options ?? new Options();
            ConfigureItems(options);

            static void prepareItemsOptions<ItemType>(
                Options.ItemsOptions<ItemType> itemsOptions,
                Func<IList<ItemType>, ISynchronizedCollection<ItemType>> itemCollectionFactory)
            {
                if (itemsOptions.Items is null) {
                    IList<ItemType> itemList;
                    CollectionChangeHandler<ItemType>.IDependencyInjectedHandler itemModificationHandler;

                    if (itemsOptions.CollectionChangeHandler is null) {
                        itemList = new List<ItemType>();
                        itemModificationHandler = new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(itemList);
                    } else {
                        itemList = itemsOptions.CollectionChangeHandler.Items;
                        itemModificationHandler = itemsOptions.CollectionChangeHandler;
                    }

                    var items = itemCollectionFactory(itemList);
                    itemsOptions.SetItems(items, itemModificationHandler);
                }
            }

            prepareItemsOptions(options.SuperItemsOptions, itemList => new SuperItemCollection(itemList, this));
            prepareItemsOptions(options.SubItemsOptions, itemList => new SubItemCollection(itemList, this));

            SubItems = options.SubItemsOptions.Items!;
            SubItemChangeHandler = options.SubItemsOptions.CollectionChangeHandler!;

            SuperItems = options.SuperItemsOptions.Items!;
            SuperItemChangeHandler = options.SuperItemsOptions.CollectionChangeHandler!;

            SynchronizationMethod = options.SynchronizationMethod
                ?? CollectionSynchronizationMethod.Sequential<TSuperItem>();
        }

        public SynchronizingCollectionBase(ICollectionSynchronizationMethod<TSuperItem, TSuperItem>? synchronizationMethod)
            : this(new Options() { SynchronizationMethod = synchronizationMethod }) { }

        public SynchronizingCollectionBase(IEqualityComparer<TSuperItem> equalityComparer)
            : this(new Options().SetSequentialSynchronizationMethod(equalityComparer)) { }

        public SynchronizingCollectionBase(IComparer<TSuperItem> equalityComparer, bool descended)
            : this(new Options().SetSortedSynchronizationMethod(equalityComparer, descended)) { }

        public SynchronizingCollectionBase()
            : this(options: null) { }

        /// <summary>
        /// Configures items options.
        /// </summary>
        /// <param name="options">The options that got passed to constructor or a new instance.</param>
        protected virtual void ConfigureItems(Options options)
        { }

        protected abstract TSubItem CreateSubItem(TSuperItem superItem);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="collectionToBeMirrored"/> are 
        /// forwarded to <see cref="GoThroughModification(ICollectionModification{TSuperItem, TSuperItem})"/>
        /// of this instance.
        /// </summary>
        /// <param name="collectionToBeMirrored">The foreign collection that is about to be mirrored related to its modifications.</param>
        /// <returns>A collection synchronization mirror.</returns>
        public SynchronizationMirror<TSuperItem> CreateSynchronizationMirror(ISynchronizedCollection<TSuperItem> collectionToBeMirrored) =>
            new SynchronizationMirror<TSuperItem>(this, collectionToBeMirrored);

        protected virtual void OnAddedItemByModification(int addedItemIndex) { }

        protected virtual void AddItemsByModification(ApplyingCollectionModifications applyingModifications)
        {
            var subSuperItemModification = applyingModifications.SuperSubItemModification;
            var subSuperItemModifiactionNewItems = subSuperItemModification.NewItems!;

            TSuperItem superItem = default!;
            int addedItemIndex = default;

            CollectionModificationIterationTools.BeginInsert(subSuperItemModification)
                /// <see cref="subSuperItemModifiactionNewItems"/> is now null checked.
                .Add((itemIndex, offset) => {
                    addedItemIndex = offset + itemIndex;
                    superItem = subSuperItemModifiactionNewItems[itemIndex];
                    SuperItemChangeHandler.InsertItem(addedItemIndex, superItem);
                })
                .Add(() => {
                    var subItem = CreateSubItem(superItem);
                    SubItemChangeHandler.InsertItem(addedItemIndex, subItem);
                })
                .Add(() => OnAddedItemByModification(addedItemIndex))
                .Iterate();
        }

        protected virtual void RemoveItemsByModification(ApplyingCollectionModifications applyingModifications)
        {
            var superItemModification = applyingModifications.SuperItemModification;
            int removedItemIndex = default;

            CollectionModificationIterationTools.BeginRemove(superItemModification)
                .Add((itemIndex, indexOffset) => {
                    removedItemIndex = itemIndex + indexOffset;
                    SuperItemChangeHandler.RemoveItem(removedItemIndex);
                })
                .Add(() => SubItemChangeHandler.RemoveItem(removedItemIndex))
                .Iterate();
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="applyingModifications"></param>
        protected virtual void MoveItemsByModification(ApplyingCollectionModifications applyingModifications)
        {
            var modification = applyingModifications.SuperSubItemModification;
            CollectionModificationIterationTools.CheckMove(modification);
            var oldItemsCount = modification.OldItems!.Count;
            SubItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
            SuperItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
        }

        protected virtual void OnReplacedItemByModification(int replacedItemIndex) { }

        /// <summary>
        /// Has default implementation: Calls <see cref="SuperItemChangeHandler"/>/<see cref="SubItemChangeHandler"/>
        /// its <see cref="CollectionChangeHandler{ItemType, NewItemType}.IDependencyInjectedHandler.ReplaceItem(int, NewItemType)"/>
        /// method when <see cref="CollectionChangeHandler{ItemType, NewItemType}.IDependencyInjectedHandler.CanReplaceItem"/> is true.
        /// </summary>
        /// <param name="applyingModifications"></param>
        protected virtual void ReplaceItemsByModification(ApplyingCollectionModifications applyingModifications)
        {
            var canReplaceSuperItem = SuperItemChangeHandler.CanReplaceItem || !(updateSuperItem is null);
            var canReplaceSubItem = SubItemChangeHandler.CanReplaceItem || !(updateSubItem is null);

            if (canReplaceSuperItem || canReplaceSubItem) {
                var superItemModification = applyingModifications.SuperItemModification;
                var iteratorBuilder = CollectionModificationIterationTools.BeginReplace(superItemModification);
                var subItemByIndex = new Dictionary<int, Lazy<TSubItem>>();
                int replacedItemIndex = 0;
                int modificationItemIndex = 0;

                void updateItem(int modificationItemIndex, int globalItemIndex)
                {
                    if (!(updateSuperItem is null)) {
                        updateSuperItem(
                            SuperItemChangeHandler.Items[globalItemIndex],
                            () => subItemByIndex[globalItemIndex].Value);
                    }

                    if (!(updateSubItem is null)) {
                        updateSubItem(
                            SubItemChangeHandler.Items[globalItemIndex],
                            () => superItemModification.NewItems![modificationItemIndex]);
                    }
                }

                iteratorBuilder.Add((innerModificationItemIndex, globalIndexOffset) => {
                    modificationItemIndex = innerModificationItemIndex;
                    replacedItemIndex = modificationItemIndex + globalIndexOffset;

                    subItemByIndex[replacedItemIndex] = new Lazy<TSubItem>(() =>
                        CreateSubItem(superItemModification.NewItems![modificationItemIndex]));
                });

                if (canReplaceSuperItem) {
                    iteratorBuilder.Add(() => {
                        var lazyNewItem = new Lazy<TSuperItem>(() =>
                            superItemModification.NewItems![modificationItemIndex]);

                        TSuperItem getNewItem() =>
                            lazyNewItem.Value;

                        if (SuperItemChangeHandler.CanReplaceItem) {
                            SuperItemChangeHandler.ReplaceItem(replacedItemIndex, getNewItem);
                        }
                    });
                }

                if (canReplaceSubItem) {
                    iteratorBuilder.Add(() => {
                        var lazyNewItem = new Lazy<TSubItem>(() =>
                            CreateSubItem(superItemModification.NewItems![modificationItemIndex]));

                        TSubItem getNewItem() =>
                            lazyNewItem.Value;

                        if (SubItemChangeHandler.CanReplaceItem) {
                            SubItemChangeHandler.ReplaceItem(replacedItemIndex, getNewItem);
                        }
                    });
                }

                iteratorBuilder.Add((modificationItemIndex, _) => updateItem(modificationItemIndex, replacedItemIndex));
                iteratorBuilder.Add(() => OnReplacedItemByModification(replacedItemIndex));
                iteratorBuilder.Iterate();
            }
        }

        protected virtual void ResetByModification(ApplyingCollectionModifications applyingModifications)
        {
            SubItemChangeHandler.Reset();
            SuperItemChangeHandler.Reset();
        }

        protected virtual void GoThroughModification(ApplyingCollectionModifications applyingModifications)
        {
            switch (applyingModifications.SuperItemModification.Action) {
                case NotifyCollectionChangedAction.Add:
                    AddItemsByModification(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItemsByModification(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItemsByModification(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItemsByModification(applyingModifications);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetByModification(applyingModifications);
                    break;
            }
        }

        protected void OnCollectionModified(CollectionModifiedEventArgs<TSuperItem, TSubItem> args)
            => CollectionModified?.Invoke(this, args);

        protected void GoThroughModification(ICollectionModification<TSuperItem, TSuperItem> superItemModification)
        {
            var oldSubItemsNewSuperItemsModification = replaceOldSuperItemsByOldSubItems(superItemModification, SubItems);
            var applyingModificationBundle = new ApplyingCollectionModifications(oldSubItemsNewSuperItemsModification, superItemModification);
            GoThroughModification(applyingModificationBundle);

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

        protected void OnCollectionSynchronizing() =>
            CollectionSynchronizing?.Invoke(this, new EventArgs());

        protected void OnCollectionSynchronized() =>
            CollectionSynchronized?.Invoke(this, new EventArgs());

        /// <summary>
        /// Synchronizes collection with <paramref name="items"/>.
        /// </summary>
        /// <param name="items"></param>
        public virtual void SynchronizeCollection(IEnumerable<TSuperItem>? items, CollectionModificationsYieldCapabilities yieldCapabilities)
        {
            var enumerator = SynchronizationMethod
                .YieldCollectionModifications(
                     SuperItemChangeHandler.Items.AsIList().ToYieldIteratorInfluencedReadOnlyList(),
                    items,
                    yieldCapabilities)
                .GetEnumerator();

            if (!enumerator.MoveNext()) {
                return;
            }

            OnCollectionSynchronizing();

            do {
                GoThroughModification(enumerator.Current);
            } while (enumerator.MoveNext());

            OnCollectionSynchronized();
        }

        public void SynchronizeCollection(IEnumerable<TSuperItem>? enumerable) =>
            SynchronizeCollection(enumerable, yieldCapabilities: CollectionModificationsYieldCapabilities.All);

        #region ICollectionSynchronizationContext<SuperItemType>

        void ICollectionSynchronizationContext<TSuperItem>.BeginCollectionSynchronization() =>
            OnCollectionSynchronizing();

        void ICollectionSynchronizationContext<TSuperItem>.GoThroughModification(ICollectionModification<TSuperItem, TSuperItem> superItemModification) =>
            GoThroughModification(superItemModification);

        void ICollectionSynchronizationContext<TSuperItem>.EndCollectionSynchronization() =>
            OnCollectionSynchronized();

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
