using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    public partial class SynchronizableCollection<ItemType> : SynchronizableCollectionBase<ItemType, ItemType>, ICollectionSynchronizationContext<ItemType>
        where ItemType : notnull
    {
        private static Options prepareOptions(ref Options? options)
        {
            options ??= new Options();
            options.CollectionChangeHandler ??= new CollectionChangeHandler<ItemType>.DependencyInjectedHandler(new List<ItemType>());
            return options;
        }

        public ICollectionSynchronizationMethod<ItemType, ItemType> SynchronizationMethod { get; private set; } = null!;

        private CollectionUpdateItemDelegate<ItemType, ItemType>? updateItem;

        public SynchronizableCollection(Options? options)
            : base(prepareOptions(ref options).CollectionChangeHandler!)
        {
            options!.SynchronizationMethod ??= CollectionSynchronizationMethod.Sequential<ItemType>();
            SynchronizationMethod = options.SynchronizationMethod;
            updateItem = options.UpdateItem;
        }

        public SynchronizableCollection(
            IList<ItemType> items,
            ICollectionSynchronizationMethod<ItemType, ItemType>? synchronizationMethod) : this(
                new Options() { SynchronizationMethod = synchronizationMethod }
                .SetItems(items))
        { }

        public SynchronizableCollection(IList<ItemType> items)
            : this(new Options().SetItems(items)) { }

        public SynchronizableCollection(ICollectionSynchronizationMethod<ItemType, ItemType> synchronizationMethod)
            : this(new Options() { SynchronizationMethod = synchronizationMethod }) { }

        public SynchronizableCollection()
            : this(options: null) { }

        public SynchronizableCollection(IList<ItemType> items, IEqualityComparer<ItemType> equalityComparer) : this(
            new Options()
            .SetItems(items)
            .SetSequentialSynchronizationMethod(equalityComparer))
        { }

        public SynchronizableCollection(IEqualityComparer<ItemType> equalityComparer) : this(
            new Options()
            .SetSequentialSynchronizationMethod(equalityComparer))
        { }

        public SynchronizableCollection(IList<ItemType> items, IComparer<ItemType> comparer, bool descended) : this(
            new Options()
            .SetItems(items)
            .SetSortedSynchronizationMethod(comparer, descended))
        { }

        public SynchronizableCollection(IComparer<ItemType> comparer, bool descended)
            : this(
                  new Options()
                  .SetSortedSynchronizationMethod(comparer, descended))
        { }

        protected virtual void AddItemByModification(ICollectionModification<ItemType, ItemType> modification)
        {
            CollectionModificationIterationTools.BeginInsert(modification)
                /// The modification is now null checked.
                .Add((modificationItemIndex, globalIndexOffset) => {
                    var superItem = modification.NewItems![modificationItemIndex];
                    var globalIndex = globalIndexOffset + modificationItemIndex;
                    CollectionChangeHandler.InsertItem(globalIndex, superItem);
                })
                .Iterate();
        }

        protected virtual void RemoveItemByModification(ICollectionModification<ItemType, ItemType> modification)
        {
            CollectionModificationIterationTools.BeginRemove(modification)
                .Add((modificationItemIndex, globalIndexOffset) => {
                    var globalIndex = globalIndexOffset + modificationItemIndex;
                    CollectionChangeHandler.RemoveItem(globalIndex);
                })
                .Iterate();
        }

        protected virtual void ReplaceItemByModification(ICollectionModification<ItemType, ItemType> modification)
        {
            CollectionModificationIterationTools.BeginReplace(modification)
                .Add((modificationItemIndex, globalIndexOffset) => {
                    var globalIndex = globalIndexOffset + modificationItemIndex;
                    var item = modification.NewItems![modificationItemIndex];

                    ItemType getItem() =>
                        item;

                    if (CollectionChangeHandler.CanReplaceItem) {
                        CollectionChangeHandler.ReplaceItem(globalIndex, getItem);
                    }

                    updateItem?.Invoke(CollectionChangeHandler.Items[globalIndex], getItem);
                })
                .Iterate();
        }

        protected virtual void MoveItemByModification(ICollectionModification<ItemType, ItemType> modification)
        {
            CollectionModificationIterationTools.CheckMove(modification);
            CollectionChangeHandler.MoveItems(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
        }

        protected virtual void ResetItemByModification(ICollectionModification<ItemType, ItemType> modification) =>
            CollectionChangeHandler.Reset();

        protected virtual void GoThroughModification(ICollectionModification<ItemType, ItemType> modification)
        {
            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    AddItemByModification(modification);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItemByModification(modification);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItemByModification(modification);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItemByModification(modification);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ResetItemByModification(modification);
                    break;
            }
        }

        internal void SynchronizeCollection(IEnumerable<ItemType> leftItems, IEnumerable<ItemType>? rightItems, CollectionModificationsYieldCapabilities yieldCapabilities, bool consumeModifications)
        {
            leftItems = leftItems ?? throw new ArgumentNullException(nameof(leftItems));
            var modifications = SynchronizationMethod.YieldCollectionModifications(leftItems, rightItems, yieldCapabilities);

            if (consumeModifications) {
                modifications = modifications.ToList();
            }

            var modificationEnumerator = modifications.GetEnumerator();

            if (!modificationEnumerator.MoveNext()) {
                return;
            }

            InvokeCollectionSynchronizing();

            do {
                var modification = modificationEnumerator.Current;
                GoThroughModification(modification);
                InvokeCollectionModified(modification);
            } while (modificationEnumerator.MoveNext());

            InvokeCollectionSynchronized();
        }

        internal void SynchronizeCollection(IEnumerable<ItemType>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities, bool consumeModifications) =>
            SynchronizeCollection(
                consumeModifications
                    ? (IEnumerable<ItemType>)Items
                    : Items.AsIList().ToYieldIteratorInfluencedReadOnlyList(),
                enumerable,
                yieldCapabilities,
                consumeModifications);

        internal void SynchronizeCollection(IEnumerable<ItemType>? enumerable, bool consumeModifications) =>
            SynchronizeCollection(enumerable, CollectionModificationsYieldCapabilities.All, consumeModifications);

        public void SynchronizeCollection(IEnumerable<ItemType>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities) =>
            SynchronizeCollection(enumerable, yieldCapabilities, consumeModifications: false);

        public void SynchronizeCollection(IEnumerable<ItemType>? enumerable) =>
            SynchronizeCollection(enumerable, yieldCapabilities: CollectionModificationsYieldCapabilities.All);

        /// <summary>
        /// Creates for this instance a collection synchronisation mirror. The collection modifications from <paramref name="toBeMirroredCollection"/> are 
        /// forwarded to <see cref="ApplyCollectionModification(ICollectionModification{SuperItemType, SuperItemType}, NotifyCollectionChangedAction[])"/>
        /// of this instance.
        /// </summary>
        /// <param name="toBeMirroredCollection">The foreign collection that is about to be mirrored related to its modifications.</param>
        /// <returns>A collection synchronization mirror.</returns>
        public SynchronizationMirror<ItemType> CreateSynchronizationMirror(ISynchronizedCollection<ItemType> toBeMirroredCollection) =>
            new SynchronizationMirror<ItemType>(this, toBeMirroredCollection);

        #region ICollectionSynchronizationContext<SuperItemType>

        void ICollectionSynchronizationContext<ItemType>.BeginCollectionSynchronization() =>
            InvokeCollectionSynchronizing();

        void ICollectionSynchronizationContext<ItemType>.GoThroughModification(ICollectionModification<ItemType, ItemType> superItemModification) =>
            GoThroughModification(superItemModification);

        void ICollectionSynchronizationContext<ItemType>.EndCollectionSynchronization() =>
            InvokeCollectionSynchronized();

        #endregion
    }
}
