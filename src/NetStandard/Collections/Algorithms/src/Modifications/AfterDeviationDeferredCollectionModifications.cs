using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Collections.ObjectModel;
using Teronis.Collections.Specialized;
using Teronis.Diagnostics;

namespace Teronis.Collections.Algorithms.Algorithms
{
    /// <summary>
    /// The algorithm creates modifications that can transform one collection into another collection.
    /// It yieldCapabilities first equal items immediatelly. After first deviation the items are going to to be cached.
    /// If left item and right item during enumeration of both are present, the left item is linked to right
    /// item, but only when the a previous right item with same comparable part has not been linked already.
    /// </summary>
    [Obsolete("This class is obsolete. Please use " + nameof(EqualityTrailingCollectionModifications) + ".")]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class AfterDeviationDeferredCollectionModifications
    {
        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            IEnumerable<RightItemType> rightItems,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem,
            IEqualityComparer<ComparablePartType> equalityComparer)
        {
            equalityComparer ??= EqualityComparer<ComparablePartType>.Default;
            var comparablePartEqualityComparer = new CommonValueContainerEqualityComparer<ComparablePartType>(equalityComparer);
            var leftItemEnumerator = leftItems.GetEnumerator();
            var rightItemEnumerator = rightItems.GetEnumerator();
            var hasLeftItem = true;
            var hasRightItem = true;
            // It will only count until a left or right item is not found
            var earlyIterationCount = 0;
            var earlyLeftValueIndex = 0;
            var earlyRightValueIndex = 0;
            // Early modifications that are synchronized or are synchronized but having
            // removing or adding modifications at the end can be returned all at once.
            // This list represents the cache for it.
            var earlyIterationModifications = new List<CollectionModification<LeftItemType, RightItemType>>();
            var leftItemIndexShifter = new ObjectEventDispatcher<CollectionModification<LeftItemType, RightItemType>>();
            ILinkedBucketList<CommonValueContainer<ComparablePartType>, LeftItemContainer<LeftItemType, RightItemType, ComparablePartType>> lateLeftItemContainers = new LinkedBucketList<CommonValueContainer<ComparablePartType>, LeftItemContainer<LeftItemType, RightItemType, ComparablePartType>>(comparablePartEqualityComparer);
            var lateRightItemContainers = new LinkedBucketList<CommonValueContainer<ComparablePartType>, RightItemContainer<LeftItemType, RightItemType, ComparablePartType>>(comparablePartEqualityComparer);
            var areEarlyIterationValuesEqual = true;

            /* Cache left and right items */
            while ((hasLeftItem && (hasLeftItem = leftItemEnumerator.MoveNext()))
                | (hasRightItem && (hasRightItem = rightItemEnumerator.MoveNext()))) {
                // Cancel if not a left and right value is available
                if (!(hasLeftItem || hasRightItem)) {
                    break;
                }

                var leftItem = hasLeftItem ? leftItemEnumerator.Current : default;
                var leftCommonValue = hasLeftItem ? getComparablePartOfLeftItem(leftItem!) : default;

                var rightItem = hasRightItem ? rightItemEnumerator.Current : default;
                var rightCommonValue = hasRightItem ? getComparablePartOfRightItem(rightItem!) : default;
                CollectionModification<LeftItemType, RightItemType>? syncedIterationModification = default;

                LeftItemContainer<LeftItemType, RightItemType, ComparablePartType> createLeftItemContainer()
                    => new LeftItemContainer<LeftItemType, RightItemType, ComparablePartType>(leftItem, leftCommonValue, earlyLeftValueIndex, leftItemIndexShifter);

                RightItemContainer<LeftItemType, RightItemType, ComparablePartType> createRightItemContainer()
                    => new RightItemContainer<LeftItemType, RightItemType, ComparablePartType>(rightItem, rightCommonValue, earlyRightValueIndex);

                if (hasLeftItem && hasRightItem) {
                    var leftItemContainer = createLeftItemContainer();
                    var rightItemContainer = createRightItemContainer();

                    // Check if: "Both items are equal" => (true supports [1,..] <-> [1,..])
                    //       AND "Left and right buckets are empty"  (false supports [1,1,2,..] <-> [2,1,..])
                    if (equalityComparer.Equals(leftCommonValue!, rightCommonValue!)
                        && ((lateRightItemContainers.Buckets.TryGetValue(rightItemContainer).Item2?.All(x => x.CachedLeftItem != null)) ?? false)) {
                        syncedIterationModification = new CollectionModification<LeftItemType, RightItemType>(
                            NotifyCollectionChangedAction.Replace, leftItem, earlyLeftValueIndex, rightItem, earlyRightValueIndex);

                        // We only move forward to back, but never back to forward, so this item, even when 
                        // it's now well positioned, needs to be checked, if it is still well positioned.
                        //var rightItemContainer = createRightItemContainer();

                        // We take use of a cache, so that we don't have to search for left item and don't replace it twice
                        rightItemContainer.CachedLeftItem = leftItemContainer;
                        lateRightItemContainers.AddLast(rightItemContainer);
                    } else {
                        lateLeftItemContainers.AddLast(leftItemContainer);
                        lateRightItemContainers.AddLast(rightItemContainer);
                        areEarlyIterationValuesEqual = false;
                    }

                    earlyIterationCount++;
                    earlyLeftValueIndex = earlyIterationCount;
                    earlyRightValueIndex = earlyIterationCount;
                } else if (hasLeftItem) {
                    if (areEarlyIterationValuesEqual) {
                        // When early iterations are synchronized, then we always need 
                        // to delete the left value at index of the greatest right value index plus one
                        var newLeftValueIndex = earlyLeftValueIndex - (earlyLeftValueIndex - earlyRightValueIndex);
                        syncedIterationModification = CollectionModification<LeftItemType, RightItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftItem, newLeftValueIndex);
                    } else {
                        lateLeftItemContainers.AddLast(createLeftItemContainer());
                    }

                    earlyLeftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual) {
                        syncedIterationModification = CollectionModification<LeftItemType, RightItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightItem, earlyRightValueIndex);
                    } else {
                        lateRightItemContainers.AddLast(createRightItemContainer());
                    }

                    earlyRightValueIndex++;
                }

                if (syncedIterationModification != null) {
                    earlyIterationModifications.Add(syncedIterationModification);
                }
            }

            foreach (var modification in earlyIterationModifications) {
                yield return modification;
            }

            // Exit only if both lists were synchronized from early on
            if (areEarlyIterationValuesEqual) {
                yield break;
            }

            foreach (var rightItemContainer in lateRightItemContainers) {
                // This index represents the current index of the right value collection.
                var rightValueIndex = rightItemContainer.ShiftedIndex;
                var rightValue = rightItemContainer.CommonValue;
                var hasCachedLeftItem = rightItemContainer.CachedLeftItem != null;
                LeftItemContainer<LeftItemType, RightItemType, ComparablePartType>? foundLeftItemContainer;

                if (hasCachedLeftItem) {
                    foundLeftItemContainer = rightItemContainer.CachedLeftItem;
                } else {
                    if (lateLeftItemContainers.TryGetBucket(CommonValueContainer<ComparablePartType>.CreateEqualComparableItem(rightValue), out var bucket) && bucket.Count != 0) {
                        var firstNode = bucket.First!;
                        foundLeftItemContainer = firstNode.Value;
                        lateLeftItemContainers.Remove(firstNode);
                    } else {
                        foundLeftItemContainer = null;
                    }
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                if (foundLeftItemContainer == null) {
                    var modification = CollectionModification<LeftItemType, RightItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                    yield return modification;
                    leftItemIndexShifter.DispatchObject(modification);
                } else {
                    var foundLeftIndex = foundLeftItemContainer.ShiftedIndex;

                    // Indexes can be equal, when not processed items are before the moved item
                    if (foundLeftIndex != rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount) {
                        // Here we deny the forward to backward move, so we have to process already replaced items, 
                        // and move them forward, because they could be now behind those skipped items, but need to
                        // be moved before them
                        if (foundLeftIndex < rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount) {
                            continue;
                        }

                        var modification = new CollectionModification<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Move, foundLeftItemContainer.LeftItem, foundLeftIndex, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                        // We move the old existing item
                        yield return modification;
                        leftItemIndexShifter.DispatchObject(modification);
                    }

                    if (!hasCachedLeftItem) {
                        // Then we replace the left item by moved item at the destination index of the moved item
                        yield return new CollectionModification<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Replace, foundLeftItemContainer.LeftItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightItemContainer.RightItem, rightValueIndex);
                    }
                }
            }

            if (!(lateLeftItemContainers.Last is null)) {
                // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
                foreach (var leftValueIndexPair in lateLeftItemContainers.Last.ListPart.GetEnumerableButReversed()) {
                    yield return CollectionModification<LeftItemType, RightItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value.LeftItem, leftValueIndexPair.Value.ShiftedIndex);
                }
            }
        }

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="LeftItemType">The type of left items.</typeparam>
        /// <typeparam name="RightItemType">The type of right items.</typeparam>
        /// <typeparam name="ComparablePartType">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> YieldCollectionModifications<LeftItemType, RightItemType, ComparablePartType>(
            IEnumerable<LeftItemType> leftItems,
            IEnumerable<RightItemType> rightItems,
            Func<LeftItemType, ComparablePartType> getComparablePartOfLeftItem,
            Func<RightItemType, ComparablePartType> getComparablePartOfRightItem) =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                EqualityComparer<ComparablePartType>.Default);


        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer"></param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            IEqualityComparer<ItemType> equalityComparer) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                equalityComparer);



        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer"></param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                EqualityComparer<ItemType>.Default);

        private class LeftItemContainer<LeftItemType, RightItemType, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            [AllowNull, MaybeNull]
            public LeftItemType LeftItem { get; private set; }

            public LeftItemContainer([AllowNull] LeftItemType leftItem, [AllowNull] CommonValueType commonValue, int index,
                ObjectEventDispatcher<CollectionModification<LeftItemType, RightItemType>> indexShifter)
                : base(commonValue, index)
            {
                LeftItem = leftItem;
                indexShifter = indexShifter ?? throw new ArgumentNullException(nameof(indexShifter));
                indexShifter.ObjectDispatch += IndexShifter_ObjectDispatch;
            }

            protected void IndexShifter_ObjectDispatch(object? sender, ObjectDispachEventArgs<CollectionModification<LeftItemType, RightItemType>> args)
            {
                var modification = args.Object;

                switch (args.Object.Action) {
                    case NotifyCollectionChangedAction.Add:
                        // When adding a 
                        if (ShiftedIndex >= modification.NewIndex) {
                            Shifts++;
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                        if (ShiftedIndex >= modification.NewIndex && ShiftedIndex < modification.OldIndex) {
                            Shifts++;
                        }

                        break;
                }
            }
        }

        private class RightItemContainer<LeftItemType, RightItemType, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            [AllowNull]
            [MaybeNull]
            public RightItemType RightItem { get; private set; }
            public LeftItemContainer<LeftItemType, RightItemType, CommonValueType>? CachedLeftItem { get; set; }

            public RightItemContainer([AllowNull] RightItemType rightItem, [AllowNull] CommonValueType commonValue, int index)
                : base(commonValue, index)
                => RightItem = rightItem;
        }

        [DebuggerDisplay(IDebuggerDisplayLibrary.FullGetDebuggerDisplayMethodPathWithParameterizedThis)]
        private class CommonValueContainer<CommonValueType> : IDebuggerDisplay
        {
            public static CommonValueContainer<CommonValueType> CreateEqualComparableItem([AllowNull] CommonValueType value)
                => new CommonValueContainer<CommonValueType>(value, -1);

            [AllowNull]
            [MaybeNull]
            public CommonValueType CommonValue { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            string IDebuggerDisplay.DebuggerDisplay => $"[{CommonValue}, {ShiftedIndex}]";

            protected CommonValueContainer([AllowNull] CommonValueType commonValue, int index)
            {
                CommonValue = commonValue;
                InitialIndex = index;
            }
        }

        private class CommonValueContainerEqualityComparer<CommonValueType> : IEqualityComparer<CommonValueContainer<CommonValueType>>
        {
            public IEqualityComparer<CommonValueType> CommonValueEqualityComparer { get; private set; }

            public CommonValueContainerEqualityComparer(IEqualityComparer<CommonValueType> commonValueEqualityComparer)
                => CommonValueEqualityComparer = commonValueEqualityComparer ?? throw new ArgumentNullException(nameof(commonValueEqualityComparer));

            public bool Equals(CommonValueContainer<CommonValueType>? x, CommonValueContainer<CommonValueType>? y)
            {
                if (x is null && y is null) {
                    return false;
                } else if (x is null || y is null) {
                    return false;
                }

                return CommonValueEqualityComparer.Equals(x.CommonValue!, y.CommonValue!);
            }

            public int GetHashCode(CommonValueContainer<CommonValueType>? obj)
            {
                if (obj is null) {
                    return 0;
                }

                return CommonValueEqualityComparer.GetHashCode(obj.CommonValue!);
            }
        }
    }
}

