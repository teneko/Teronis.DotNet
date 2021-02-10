using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    /// <summary>
    /// A synchronizing collection that is itself a view model.
    /// Bindable collections are <see cref="SubItems"/> and <see cref="SuperItems"/>.
    /// Remember: All childs are parents (because child extends parent), so all childs are a subset of parent / parent is a superset of child.
    /// </summary>
    /// <typeparam name="SubItemType"></typeparam>
    /// <typeparam name="SuperItemType"></typeparam>
    public abstract partial class SynchronizingCollection<SubItemType, SuperItemType> : INotifyCollectionModification<SubItemType, SuperItemType>, ICollectionSynchronizationContext<SuperItemType>
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        private static CollectionModification<SubItemType, SuperItemType> replaceOldSuperItemsByOldSubItems(
            ICollectionModification<SuperItemType, SuperItemType> superItemsSuperItemsModification,
            IReadOnlyCollection<SubItemType> subItems)
        {
            var oldItems = superItemsSuperItemsModification.GetItemsBeginningFromOldIndex(subItems);
            var subItemsSuperItemsModification = superItemsSuperItemsModification.CopyWithOtherItems(oldItems, superItemsSuperItemsModification.NewItems);
            return subItemsSuperItemsModification;
        }

        public event EventHandler? CollectionSynchronizing;
        public event EventHandler? CollectionSynchronized;
        public event NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType>? CollectionModified;

        public ISynchronizedCollection<SubItemType> SubItems { get; }
        public ISynchronizedCollection<SuperItemType> SuperItems { get; }
        public ICollectionSynchronizationMethod<SuperItemType, SuperItemType> SynchronizationMethod { get; }

        protected CollectionChangeHandler<SubItemType>.IDependencyInjectedHandler SubItemChangeHandler { get; }
        protected CollectionChangeHandler<SuperItemType>.IDependencyInjectedHandler SuperItemChangeHandler { get; }

        public SynchronizingCollection(Options? options)
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
                ?? CollectionSynchronizationMethod.Sequential<SuperItemType>();
        }

        public SynchronizingCollection()
            : this(options: null) { }

        protected virtual void ConfigureItems(Options options)
        { }

        protected abstract SubItemType CreateSubItem(SuperItemType superItem);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="toBeMirroredCollection"/> are 
        /// forwarded to <see cref="ApplyCollectionModification(ICollectionModification{SuperItemType, SuperItemType}, NotifyCollectionChangedAction[])"/>
        /// of this instance.
        /// </summary>
        /// <param name="toBeMirroredCollection">The foreign collection that is about to be mirrored related to its modifications.</param>
        /// <returns>A collection synchronization mirror.</returns>
        public SynchronizationMirror<SuperItemType> CreateSynchronizationMirror(ISynchronizedCollection<SuperItemType> toBeMirroredCollection) =>
            new SynchronizationMirror<SuperItemType>(this, toBeMirroredCollection);

        protected virtual void AddItemByModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            var subSuperItemModification = modificationBundle.SubSuperItemModification;
            var subSuperItemModifiactionNewItems = subSuperItemModification.NewItems!;

            SuperItemType superItem = default!;
            int offsetItemIndex = default;

            CollectionModificationIterationTools.BeginInsert(subSuperItemModification)
                /// <see cref="subSuperItemModifiactionNewItems"/> is now null checked.
                .Add((itemIndex, offset) => {
                    superItem = subSuperItemModifiactionNewItems[itemIndex];
                    offsetItemIndex = offset + itemIndex;
                    SuperItemChangeHandler.InsertItem(offsetItemIndex, superItem);
                })
                .Add((itemIndex, offset) => {
                    var subItem = CreateSubItem(superItem);
                    SubItemChangeHandler.InsertItem(offsetItemIndex, subItem);
                })
                .Iterate();
        }

        protected virtual void RemoveItemByModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            var superItemModification = modificationBundle.SuperItemModification;
            int offsetItemIndex = default;

            CollectionModificationIterationTools.BeginRemove(superItemModification)
                .Add((itemIndex, indexOffset) => {
                    offsetItemIndex = itemIndex + indexOffset;
                    SuperItemChangeHandler.RemoveItem(offsetItemIndex);
                })
                .Add((itemIndex, _) => {
                    SubItemChangeHandler.RemoveItem(offsetItemIndex);
                })
                .Iterate();
        }

        /// <summary>
        /// Does regards <see cref="ObservableCollection{T}.Move(int, int)"/>, otherwise 
        /// it fallbacks to <see cref="IListGenericExtensions.Move{T}(IList{T}, int, int)"/>
        /// </summary>
        /// <param name="modificationBundle"></param>
        protected virtual void MoveItemByModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            var modification = modificationBundle.SubSuperItemModification;
            CollectionModificationIterationTools.CheckMove(modification);
            var oldItemsCount = modification.OldItems!.Count;
            SubItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
            SuperItemChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, oldItemsCount);
        }

        protected virtual void ConfigureItemUpdate(ItemUpdateOptions options) { }

        /// <summary>
        /// Has default implementation: Calls <see cref="SuperItemChangeHandler"/>/<see cref="SubItemChangeHandler"/>
        /// its <see cref="CollectionChangeHandler{ItemType, NewItemType}.IDependencyInjectedHandler.ReplaceItem(int, NewItemType)"/>
        /// method when <see cref="CollectionChangeHandler{ItemType, NewItemType}.IDependencyInjectedHandler.CanReplaceItem"/> is true.
        /// </summary>
        /// <param name="modificationBundle"></param>
        protected virtual void ReplaceItemByModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            var itemUpdateOptions = new ItemUpdateOptions();
            ConfigureItemUpdate(itemUpdateOptions);

            var canReplaceSuperItem = SuperItemChangeHandler.CanReplaceItem || !(itemUpdateOptions.UpdateSuperItem is null);
            var canReplaceSubItem = SubItemChangeHandler.CanReplaceItem || !(itemUpdateOptions.UpdateSubItem is null);

            if (canReplaceSuperItem || canReplaceSubItem) {
                var superItemModification = modificationBundle.SuperItemModification;
                var iteratorBuilder = CollectionModificationIterationTools.BeginReplace(superItemModification);

                void updateItem(int globalItemIndex)
                {
                    if (!(itemUpdateOptions.UpdateSuperItem is null)) {
                        itemUpdateOptions.UpdateSuperItem(
                            SuperItemChangeHandler.Items[globalItemIndex],
                            SubItemChangeHandler.Items[globalItemIndex]);
                    }

                    if (!(itemUpdateOptions.UpdateSubItem is null)) {
                        itemUpdateOptions.UpdateSubItem(
                            SubItemChangeHandler.Items[globalItemIndex],
                            SuperItemChangeHandler.Items[globalItemIndex]);
                    }
                }

                if (canReplaceSuperItem) {
                    iteratorBuilder.Add((modificationItemIndex, globalIndexOffset) => {
                        var globalIndex = modificationItemIndex + globalIndexOffset;

                        var lazyNewItem = new Lazy<SuperItemType>(() =>
                            superItemModification.NewItems![modificationItemIndex]);

                        SuperItemType getNewItem() =>
                            lazyNewItem.Value;

                        if (SuperItemChangeHandler.CanReplaceItem) {
                            SuperItemChangeHandler.ReplaceItem(globalIndex, getNewItem);
                        }

                        if (!canReplaceSubItem) {
                            updateItem(globalIndex);
                        }
                    });
                }

                if (canReplaceSubItem) {
                    iteratorBuilder.Add((modificationItemIndex, globalIndexOffset) => {
                        var globalIndex = modificationItemIndex + globalIndexOffset;

                        var lazyNewItem = new Lazy<SubItemType>(() =>
                            CreateSubItem(superItemModification.NewItems![modificationItemIndex]));

                        SubItemType getNewItem() =>
                            lazyNewItem.Value;

                        if (SubItemChangeHandler.CanReplaceItem) {
                            SubItemChangeHandler.ReplaceItem(globalIndex, getNewItem);
                        }

                        updateItem(globalIndex);
                    });
                }

                iteratorBuilder.Iterate();
            }
        }

        protected virtual void ResetByModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            var modification = modificationBundle.SubSuperItemModification;
            var newSuperItems = modification.NewItems ?? throw new ArgumentNullException(nameof(modification.NewItems));
            var newSubItems = newSuperItems.Select(x => CreateSubItem(x));

            SubItemChangeHandler.Reset();
            SuperItemChangeHandler.Reset();
        }

        protected virtual void GoThroughModification(ApplyingCollectionModificationBundle modificationBundle)
        {
            switch (modificationBundle.SuperItemModification.Action) {
                case NotifyCollectionChangedAction.Remove:
                    RemoveItemByModification(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Add:
                    AddItemByModification(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItemByModification(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItemByModification(modificationBundle);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetByModification(modificationBundle);
                    break;
            }
        }

        protected void OnCollectionModified(CollectionModifiedEventArgs<SubItemType, SuperItemType> args)
            => CollectionModified?.Invoke(this, args);

        protected virtual void GoThroughModification(ICollectionModification<SuperItemType, SuperItemType> superItemModification)
        {
            var oldSubItemsNewSuperItemsModification = replaceOldSuperItemsByOldSubItems(superItemModification, SubItems);
            var applyingModificationBundle = new ApplyingCollectionModificationBundle(oldSubItemsNewSuperItemsModification, superItemModification);
            GoThroughModification(applyingModificationBundle);

            /*  We want transform new-super-items to new-sub-items because 
             *  they have been created previously if any is existing. */
            var newSubItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(SubItems);
            var oldSubItemsNewSubItemsModification = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldSubItemsNewSuperItemsModification.OldItems, newSubItems);

            var collectionModifiedArgs = new CollectionModifiedEventArgs<SubItemType, SuperItemType>(
                oldSubItemsNewSubItemsModification,
                applyingModificationBundle.SubSuperItemModification,
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
        public virtual void SynchronizeCollection(IEnumerable<SuperItemType>? items, CollectionModificationsYieldCapabilities yieldCapabilities)
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

        public void SynchronizeCollection(IEnumerable<SuperItemType>? enumerable) =>
            SynchronizeCollection(enumerable, yieldCapabilities: CollectionModificationsYieldCapabilities.All);

        #region ICollectionSynchronizationContext<SuperItemType>

        void ICollectionSynchronizationContext<SuperItemType>.BeginCollectionSynchronization() =>
            OnCollectionSynchronizing();

        void ICollectionSynchronizationContext<SuperItemType>.GoThroughModification(ICollectionModification<SuperItemType, SuperItemType> superItemModification) =>
            GoThroughModification(superItemModification);

        void ICollectionSynchronizationContext<SuperItemType>.EndCollectionSynchronization() =>
            OnCollectionSynchronized();

        #endregion

        protected readonly struct ApplyingCollectionModificationBundle
        {
            public ICollectionModification<SubItemType, SuperItemType> SubSuperItemModification { get; }
            public ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }

            public ApplyingCollectionModificationBundle(
                ICollectionModification<SubItemType, SuperItemType> subSuperItemModification,
                ICollectionModification<SuperItemType, SuperItemType> superItemModification)
            {
                SubSuperItemModification = subSuperItemModification;
                SuperItemModification = superItemModification;
            }
        }

        protected sealed class ItemUpdateOptions
        {
            /// <summary>
            /// If not null it is called in <see cref="SynchronizingCollection{SubItemType, SuperItemType}.ReplaceItemByModification(ApplyingCollectionModificationBundle)"/>
            /// but after the items could have been replaced.
            /// </summary>
            public CollectionUpdateItemDelegate<SuperItemType, SubItemType>? UpdateSuperItem { get; set; }
            /// <summary>
            /// If not null it is called by <see cref="SynchronizingCollection{SubItemType, SuperItemType}.ReplaceItemByModification(ApplyingCollectionModificationBundle)"/>
            /// but after the items could have been replaced.
            /// <br/>
            /// <br/>(!) Take into regard, that <see cref="UpdateSuperItem"/> is called at first if not null.
            /// </summary>
            public CollectionUpdateItemDelegate<SubItemType, SuperItemType>? UpdateSubItem { get; set; }
        }
    }
}
