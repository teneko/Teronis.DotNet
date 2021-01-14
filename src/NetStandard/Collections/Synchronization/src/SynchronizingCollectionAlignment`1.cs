using System.Collections.Generic;
using System.Linq;
using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public sealed class SynchronizingCollectionAlignment<ItemType>
    {
        private readonly IEqualityComparer<ItemType>? equalityComparer;
        private readonly IComparer<ItemType>? comparer;
        private readonly SynchronizingCollectionOrder assumedOrder;

        internal SynchronizingCollectionAlignment(IEqualityComparer<ItemType> equalityComparer)
        {
            this.equalityComparer = equalityComparer ?? EqualityComparer<ItemType>.Default;
            assumedOrder = SynchronizingCollectionOrder.Unordered;
        }

        internal SynchronizingCollectionAlignment(IComparer<ItemType> comparer, SynchronizingCollectionOrder assumedOrder)
        {
            this.comparer = comparer ?? Comparer<ItemType>.Default;
            this.assumedOrder = assumedOrder;
        }

        public IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications(IEnumerable<ItemType> superItems, IEnumerable<ItemType>? items) {
            items ??= Enumerable.Empty<ItemType>();
            IEnumerable<CollectionModification<ItemType, ItemType>> modifications;

            if (assumedOrder == SynchronizingCollectionOrder.Unordered) {
                modifications = CollectionModificationsObsolete.YieldCollectionModifications(superItems, items, equalityComparer!);
            } else if (assumedOrder == SynchronizingCollectionOrder.Ascending) {
                modifications = SortedCollectionModifications.YieldCollectionModifications(superItems, items, SortedCollectionModificationsOrder.Ascending, comparer!);
            } else {
                modifications = SortedCollectionModifications.YieldCollectionModifications(superItems, items, SortedCollectionModificationsOrder.Descending, comparer!);
            }

            return modifications;
        }
    }
}
