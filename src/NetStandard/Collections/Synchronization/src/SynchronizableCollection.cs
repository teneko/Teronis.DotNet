using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Extensions;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizableCollection<ItemType> : List<ItemType>, ISynchronizableCollection<ItemType>
        where ItemType : notnull
    {
        public ICollectionSynchronizationMethod<ItemType> SynchronizationMethod { get; }

        public SynchronizableCollection()
            : base() =>
            SynchronizationMethod = CollectionSynchronizationMethod.Sequential<ItemType>();

        public SynchronizableCollection(IEnumerable<ItemType> collection)
            : base(collection) =>
            SynchronizationMethod = CollectionSynchronizationMethod.Sequential<ItemType>();

        public SynchronizableCollection(int capacity)
            : base(capacity) =>
            SynchronizationMethod = CollectionSynchronizationMethod.Sequential<ItemType>();

        public SynchronizableCollection(ICollectionSynchronizationMethod<ItemType> synchronizationMethod)
            : base() =>
            SynchronizationMethod = synchronizationMethod;

        public SynchronizableCollection(ICollectionSynchronizationMethod<ItemType> synchronizationMethod, IEnumerable<ItemType> collection)
            : base(collection) =>
            SynchronizationMethod = synchronizationMethod;

        public SynchronizableCollection(ICollectionSynchronizationMethod<ItemType> synchronizationMethod, int capacity)
            : base(capacity) =>
            SynchronizationMethod = synchronizationMethod;

        internal void SynchronizeCollection(IEnumerable<ItemType> leftItems, IEnumerable<ItemType>? rightItems, CollectionModificationsYieldCapabilities yieldCapabilities, bool consumeModifications)
        {
            leftItems = leftItems ?? throw new ArgumentNullException(nameof(leftItems));
            var modifications = SynchronizationMethod.YieldCollectionModifications(leftItems, rightItems, yieldCapabilities);

            if (consumeModifications) {
                modifications = modifications.ToList();
            }

            foreach (var modification in modifications) {
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
                        this.Move(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Clear();
                        break;
                }
            }
        }

        internal void SynchronizeCollection(IEnumerable<ItemType>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities, bool consumeModifications) =>
            SynchronizeCollection(
                consumeModifications
                ? (IReadOnlyList<ItemType>)this
                : this.AsIReadOnlyList().ToYieldIteratorInfluencedReadOnlyList(),
                enumerable,
                yieldCapabilities,
                consumeModifications);

        internal void SynchronizeCollection(IEnumerable<ItemType>? enumerable, bool consumeModifications) =>
            SynchronizeCollection(enumerable, CollectionModificationsYieldCapabilities.All, consumeModifications);

        public void SynchronizeCollection(IEnumerable<ItemType>? enumerable, CollectionModificationsYieldCapabilities yieldCapabilities) =>
            SynchronizeCollection(enumerable, yieldCapabilities, consumeModifications: false);

        public void SynchronizeCollection(IEnumerable<ItemType>? enumerable) =>
            SynchronizeCollection(enumerable, consumeModifications: false);
    }
}
