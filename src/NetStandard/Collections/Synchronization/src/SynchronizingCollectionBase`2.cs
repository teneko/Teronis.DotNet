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
    /// A synchronizing collection.
    /// <br/>Bindable collections are <see cref="SubItems"/> and <see cref="SuperItems"/>.
    /// <br/>Remember: All childs are parents (because child extends parent), so all childs are a subset of parent / parent is a superset of child.
    /// </summary>
    /// <typeparam name="SubItemType"></typeparam>
    /// <typeparam name="SuperItemType"></typeparam>
    public abstract partial class SynchronizingCollectionBase<SuperItemType, SubItemType> : INotifyCollectionModification<SuperItemType, SubItemType>, ICollectionSynchronizationContext<SuperItemType>
        where SubItemType : notnull
        where SuperItemType : notnull
    {
        private static CollectionModification<SuperItemType, SubItemType> replaceOldSuperItemsByOldSubItems(
            ICollectionModification<SuperItemType, SuperItemType> superItemsSuperItemsModification,
            IReadOnlyCollection<SubItemType> subItems)
        {
            var oldItems = superItemsSuperItemsModification.GetItemsBeginningFromOldIndex(subItems);
            var subItemsSuperItemsModification = superItemsSuperItemsModification.CopyWithOtherItems(oldItems, superItemsSuperItemsModification.NewItems);
            return subItemsSuperItemsModification;
        }

        public event EventHandler? CollectionSynchronizing;
        public event EventHandler? CollectionSynchronized;
        public event NotifyCollectionModifiedEventHandler<SuperItemType, SubItemType>? CollectionModified;

        public ISynchronizedCollection<SubItemType> SubItems { get; }
        public ISynchronizedCollection<SuperItemType> SuperItems { get; }
        public ICollectionSynchronizationMethod<SuperItemType, SuperItemType> SynchronizationMethod { get; }

        protected CollectionChangeHandler<SubItemType>.IDependencyInjectedHandler SubItemChangeHandler { get; }
        protected CollectionChangeHandler<SuperItemType>.IDependencyInjectedHandler SuperItemChangeHandler { get; }

        private CollectionUpdateItemDelegate<SuperItemType, SubItemType>? updateSuperItem;
        private CollectionUpdateItemDelegate<SubItemType, SuperItemType>? updateSubItem;

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
                ?? CollectionSynchronizationMethod.Sequential<SuperItemType>();
        }

        public SynchronizingCollectionBase(ICollectionSynchronizationMethod<SuperItemType, SuperItemType>? synchronizationMethod)
            : this(new Options() { SynchronizationMethod = synchronizationMethod }) { }

        public SynchronizingCollectionBase(IEqualityComparer<SuperItemType> equalityComparer)
            : this(new Options().SetSequentialSynchronizationMethod(equalityComparer)) { }

        public SynchronizingCollectionBase(IComparer<SuperItemType> equalityComparer, bool descended)
            : this(new Options().SetSortedSynchronizationMethod(equalityComparer, descended)) { }

        public SynchronizingCollectionBase()
            : this(options: null) { }

        /// <summary>
        /// Configures items options.
        /// </summary>
        /// <param name="options">The options that got passed to constructor or a new instance.</param>
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

        protected virtual void OnAddedItemByModification(int addedItemIndex) { }

        protected virtual void AddItemsByModification(ApplyingCollectionModifications applyingModifications)
        {
            var subSuperItemModification = applyingModifications.SuperSubItemModification;
            var subSuperItemModifiactionNewItems = subSuperItemModification.NewItems!;

            SuperItemType superItem = default!;
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
                var subItemByIndex = new Dictionary<int, Lazy<SubItemType>>();
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

                    subItemByIndex[replacedItemIndex] = new Lazy<SubItemType>(() =>
                        CreateSubItem(superItemModification.NewItems![modificationItemIndex]));
                });

                if (canReplaceSuperItem) {
                    iteratorBuilder.Add(() => {
                        var lazyNewItem = new Lazy<SuperItemType>(() =>
                            superItemModification.NewItems![modificationItemIndex]);

                        SuperItemType getNewItem() =>
                            lazyNewItem.Value;

                        if (SuperItemChangeHandler.CanReplaceItem) {
                            SuperItemChangeHandler.ReplaceItem(replacedItemIndex, getNewItem);
                        }
                    });
                }

                if (canReplaceSubItem) {
                    iteratorBuilder.Add(() => {
                        var lazyNewItem = new Lazy<SubItemType>(() =>
                            CreateSubItem(superItemModification.NewItems![modificationItemIndex]));

                        SubItemType getNewItem() =>
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

        protected void OnCollectionModified(CollectionModifiedEventArgs<SuperItemType, SubItemType> args)
            => CollectionModified?.Invoke(this, args);

        protected void GoThroughModification(ICollectionModification<SuperItemType, SuperItemType> superItemModification)
        {
            var oldSubItemsNewSuperItemsModification = replaceOldSuperItemsByOldSubItems(superItemModification, SubItems);
            var applyingModificationBundle = new ApplyingCollectionModifications(oldSubItemsNewSuperItemsModification, superItemModification);
            GoThroughModification(applyingModificationBundle);

            /*  We want transform new-super-items to new-sub-items because 
             *  they have been created previously if any is existing. */
            var newSubItems = oldSubItemsNewSuperItemsModification.GetItemsBeginningFromNewIndex(SubItems);
            var oldSubItemsNewSubItemsModification = oldSubItemsNewSuperItemsModification.CopyWithOtherItems(oldSubItemsNewSuperItemsModification.OldItems, newSubItems);

            var collectionModifiedArgs = new CollectionModifiedEventArgs<SuperItemType, SubItemType>(
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

        protected readonly struct ApplyingCollectionModifications
        {
            public ICollectionModification<SuperItemType, SubItemType> SuperSubItemModification { get; }
            public ICollectionModification<SuperItemType, SuperItemType> SuperItemModification { get; }

            public ApplyingCollectionModifications(
                ICollectionModification<SuperItemType, SubItemType> subSuperItemModification,
                ICollectionModification<SuperItemType, SuperItemType> superItemModification)
            {
                SuperSubItemModification = subSuperItemModification;
                SuperItemModification = superItemModification;
            }
        }
    }
}
