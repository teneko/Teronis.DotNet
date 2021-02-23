using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teronis.Collections.Algorithms.Modifications
{
    public abstract class CollectionSynchronizationMethod<LeftItemType, RightItemType, ComparableItemType> : ICollectionSynchronizationMethod<LeftItemType, RightItemType>
        where ComparableItemType : notnull
    {
        public CollectionSequenceType SequenceType { get; }

        protected Func<LeftItemType, ComparableItemType> GetComparablePartOfLeftItem { get; }
        protected Func<RightItemType, ComparableItemType> GetComparablePartOfRightItem { get; }

        protected CollectionSynchronizationMethod(
            CollectionSequenceType sequenceType,
            Func<LeftItemType, ComparableItemType> getComparablePartOfLeftItem,
            Func<RightItemType, ComparableItemType> getComparablePartOfRightItem)
        {
            SequenceType = sequenceType;
            GetComparablePartOfLeftItem = getComparablePartOfLeftItem ?? throw new ArgumentNullException(nameof(getComparablePartOfLeftItem));
            GetComparablePartOfRightItem = getComparablePartOfRightItem ?? throw new ArgumentNullException(nameof(getComparablePartOfRightItem));
        }

        /// <summary>
        /// Checks arguments when yielding collection modifications.
        /// </summary>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="leftItems"/> is null.</exception>
        protected void CheckArgumentsWhenYieldingCollectionModifications(IEnumerable<LeftItemType> leftItems, [NotNull] ref IEnumerable<RightItemType>? rightItems)
        {
            if (leftItems is null) {
                throw new ArgumentNullException(nameof(leftItems));
            }

            rightItems ??= Enumerable.Empty<RightItemType>();
        }

        /// <summary>
        /// Yields modifications that can transform one collection into another collection.
        /// </summary>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="yieldCapabilities">The yield capabilities, e.g. only insert or only remove.</param>
        /// <returns>The collection modifications.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="leftItems"/> is null.</exception>
        public abstract IEnumerable<CollectionModification<RightItemType, LeftItemType>> YieldCollectionModifications(
            IEnumerable<LeftItemType> leftItems,
            IEnumerable<RightItemType>? rightItems,
            CollectionModificationsYieldCapabilities yieldCapabilities);

        public class Sequential : CollectionSynchronizationMethod<LeftItemType, RightItemType, ComparableItemType>
        {
            public IEqualityComparer<ComparableItemType> EqualityComparer { get; }

            public Sequential(
                Func<LeftItemType, ComparableItemType> getComparablePartOfLeftItem,
                Func<RightItemType, ComparableItemType> getComparablePartOfRightItem,
                IEqualityComparer<ComparableItemType> equalityComparer)
                : base(CollectionSequenceType.Sequential, getComparablePartOfLeftItem, getComparablePartOfRightItem) =>
                EqualityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

            public override IEnumerable<CollectionModification<RightItemType, LeftItemType>> YieldCollectionModifications(
                IEnumerable<LeftItemType> leftItems,
                IEnumerable<RightItemType>? rightItems,
                CollectionModificationsYieldCapabilities yieldCapabilities)
            {
                CheckArgumentsWhenYieldingCollectionModifications(leftItems, ref rightItems);

                return EqualityTrailingCollectionModifications.YieldCollectionModifications(
                    leftItems,
                    GetComparablePartOfLeftItem,
                    rightItems,
                    GetComparablePartOfRightItem,
                    EqualityComparer,
                    yieldCapabilities);
            }
        }

        public class Sorted : CollectionSynchronizationMethod<LeftItemType, RightItemType, ComparableItemType>
        {
            public SortedCollectionOrder CollectionOrder { get; }
            public IComparer<ComparableItemType> Comparer { get; }

            public Sorted(
                Func<LeftItemType, ComparableItemType> getComparablePartOfLeftItem,
                Func<RightItemType, ComparableItemType> getComparablePartOfRightItem,
                SortedCollectionOrder collectionOrder,
                IComparer<ComparableItemType> comparer)
                : base(CollectionSequenceType.Sequential, getComparablePartOfLeftItem, getComparablePartOfRightItem)
            {
                CollectionOrder = collectionOrder;
                Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            }

            public override IEnumerable<CollectionModification<RightItemType, LeftItemType>> YieldCollectionModifications(
                IEnumerable<LeftItemType> leftItems,
                IEnumerable<RightItemType>? rightItems,
                CollectionModificationsYieldCapabilities yieldCapabilities)
            {
                CheckArgumentsWhenYieldingCollectionModifications(leftItems, ref rightItems);

                return SortedCollectionModifications.YieldCollectionModifications(
                    leftItems,
                    GetComparablePartOfLeftItem,
                    rightItems,
                    GetComparablePartOfRightItem,
                    CollectionOrder,
                    Comparer,
                    yieldCapabilities);
            }
        }
    }
}
