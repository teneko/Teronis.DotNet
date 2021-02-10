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
        public ICollectionSynchronizationMethod<ItemType, ItemType> SynchronizationMethod { get; private set; } = null!;

        public SynchronizableCollection(
            IList<ItemType> items,
            ICollectionSynchronizationMethod<ItemType, ItemType>? synchronizationMethod)
            : base(items) =>
            onConstruction(synchronizationMethod);

        public SynchronizableCollection(IList<ItemType> items)
            : base(items) =>
            onConstruction(synchronizationMethod: null);

        public SynchronizableCollection(ICollectionSynchronizationMethod<ItemType> synchronizationMethod) =>
            onConstruction(synchronizationMethod);

        public SynchronizableCollection() =>
            onConstruction(equalityComparer: null);

        public SynchronizableCollection(IList<ItemType> items, IEqualityComparer<ItemType> equalityComparer)
            : base(items) =>
            onConstruction(equalityComparer);

        public SynchronizableCollection(IEqualityComparer<ItemType> equalityComparer) =>
            onConstruction(equalityComparer);

        public SynchronizableCollection(IList<ItemType> items, IComparer<ItemType> comparer, bool descended)
            : base(items) =>
            onConstruction(comparer, descended);

        public SynchronizableCollection(IComparer<ItemType> comparer, bool descended) =>
            onConstruction(comparer, descended);

        private void onConstruction(ICollectionSynchronizationMethod<ItemType, ItemType>? synchronizationMethod) =>
            SynchronizationMethod = synchronizationMethod ?? CollectionSynchronizationMethod.Sequential<ItemType>();

        private void onConstruction(IEqualityComparer<ItemType>? equalityComparer) =>
            SynchronizationMethod = CollectionSynchronizationMethod.Sequential(equalityComparer ?? EqualityComparer<ItemType>.Default);

        private void onConstruction(IComparer<ItemType> comparer, bool descended)
        {
            SynchronizationMethod = descended
                    ? CollectionSynchronizationMethod.Descending(comparer)
                    : CollectionSynchronizationMethod.Ascending(comparer);
        }

        protected virtual void GoThroughModification(ICollectionModification<ItemType, ItemType> modification)
        {
            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add: {
                        var index = modification.NewIndex;

                        foreach (var newItem in modification.NewItems!) {
                            Insert(index++, newItem);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove: {
                        var index = modification.OldIndex + modification.OldItems!.Count - 1;

                        foreach (var newItem in modification.OldItems) {
                            RemoveAt(index--);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Replace: {
                        var index = modification.NewIndex;

                        foreach (var newItem in modification.NewItems!) {
                            this[index] = newItem;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItems(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
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
