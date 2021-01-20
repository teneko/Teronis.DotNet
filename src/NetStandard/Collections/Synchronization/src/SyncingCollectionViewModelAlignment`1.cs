using System.Collections.Generic;
using System.Linq;
using Teronis.Collections.Algorithms;
using Teronis.Collections.Algorithms.Algorithms;

namespace Teronis.Collections.Synchronization
{
    public sealed class SyncingCollectionViewModelAlignment<ItemType>
    {
        private readonly IEqualityComparer<ItemType>? equalityComparer;
        private readonly IComparer<ItemType>? comparer;
        private readonly SyncingCollectionViewModelOrder assumedOrder;

        internal SyncingCollectionViewModelAlignment(IEqualityComparer<ItemType> equalityComparer)
        {
            this.equalityComparer = equalityComparer ?? EqualityComparer<ItemType>.Default;
            assumedOrder = SyncingCollectionViewModelOrder.Unordered;
        }

        internal SyncingCollectionViewModelAlignment(IComparer<ItemType> comparer, SyncingCollectionViewModelOrder assumedOrder)
        {
            this.comparer = comparer ?? Comparer<ItemType>.Default;
            this.assumedOrder = assumedOrder;
        }

        public IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications(IEnumerable<ItemType> superItems, IEnumerable<ItemType>? items) {
            items ??= Enumerable.Empty<ItemType>();
            IEnumerable<CollectionModification<ItemType, ItemType>> modifications;

            if (assumedOrder == SyncingCollectionViewModelOrder.Unordered) {
                modifications = EqualityTrailingCollectionModifications.YieldCollectionModifications(superItems, items, equalityComparer!);
            } else if (assumedOrder == SyncingCollectionViewModelOrder.Ascending) {
                modifications = SortedCollectionModifications.YieldCollectionModifications(superItems, items, SortedCollectionModificationsOrder.Ascending, comparer!);
            } else {
                modifications = SortedCollectionModifications.YieldCollectionModifications(superItems, items, SortedCollectionModificationsOrder.Descending, comparer!);
            }

            return modifications;
        }
    }
}
