using System.Collections.Generic;
using System.Linq;
using Teronis.Collections.Algorithms.Algorithms;

namespace Teronis.Collections.Algorithms.Modifications
{
    public sealed class CollectionSynchronizationMethod<ItemType>
        where ItemType : notnull
    {
        private readonly IEqualityComparer<ItemType>? equalityComparer;
        private readonly IComparer<ItemType>? comparer;
        private readonly CollectionSequenceType sequenceType;

        internal CollectionSynchronizationMethod(IEqualityComparer<ItemType> equalityComparer)
        {
            this.equalityComparer = equalityComparer ?? EqualityComparer<ItemType>.Default;
            sequenceType = CollectionSequenceType.Sequential;
        }

        internal CollectionSynchronizationMethod(IComparer<ItemType> comparer, CollectionSequenceType sequenceType)
        {
            this.comparer = comparer ?? Comparer<ItemType>.Default;
            this.sequenceType = sequenceType;
        }

        /// <summary>
        /// Yield modifications that can transform one collection into another collection.
        /// </summary>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>Modifications</returns>
        public IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications(
            IEnumerable<ItemType> leftItems, 
            IEnumerable<ItemType>? rightItems, 
            CollectionModificationsYieldCapabilities yieldCapabilities)
        {
            rightItems ??= Enumerable.Empty<ItemType>();

            if (sequenceType == CollectionSequenceType.Sequential) {
                return EqualityTrailingCollectionModifications.YieldCollectionModifications(leftItems, rightItems, equalityComparer, yieldCapabilities);
            } else if (sequenceType == CollectionSequenceType.Ascending) {
                return SortedCollectionModifications.YieldCollectionModifications(leftItems, rightItems, SortedCollectionOrder.Ascending, comparer!);
            } else {
                return SortedCollectionModifications.YieldCollectionModifications(leftItems, rightItems, SortedCollectionOrder.Descending, comparer!);
            }
        }

        public IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications(IEnumerable<ItemType> superItems, IEnumerable<ItemType>? items) =>
            YieldCollectionModifications(superItems, items, CollectionModificationsYieldCapabilities.All);
    }
}
