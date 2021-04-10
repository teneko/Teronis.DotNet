// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

namespace Teronis.Collections.Algorithms.Modifications
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
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <param name="equalityComparer"></param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            Func<TLeftItem, TComparablePart> getComparablePartOfLeftItem,
            IEnumerable<TRightItem> rightItems,
            Func<TRightItem, TComparablePart> getComparablePartOfRightItem,
            IEqualityComparer<TComparablePart> equalityComparer)
        {
            var comparablePartEqualityComparer = new CommonValueContainerEqualityComparer<TComparablePart>(equalityComparer);
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
            var earlyIterationModifications = new List<CollectionModification<TRightItem, TLeftItem>>();
            var leftItemIndexShifter = new ObjectEventDispatcher<CollectionModification<TRightItem, TLeftItem>>();
            ILinkedBucketList<CommonValueContainer<TComparablePart>, LeftItemContainer<TLeftItem, TRightItem, TComparablePart>> lateLeftItemContainers = new LinkedBucketList<CommonValueContainer<TComparablePart>, LeftItemContainer<TLeftItem, TRightItem, TComparablePart>>(comparablePartEqualityComparer);
            var lateRightItemContainers = new LinkedBucketList<CommonValueContainer<TComparablePart>, RightItemContainer<TLeftItem, TRightItem, TComparablePart>>(comparablePartEqualityComparer);
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
                CollectionModification<TRightItem, TLeftItem>? syncedIterationModification = default;

                LeftItemContainer<TLeftItem, TRightItem, TComparablePart> createLeftItemContainer()
                    => new LeftItemContainer<TLeftItem, TRightItem, TComparablePart>(leftItem, leftCommonValue, earlyLeftValueIndex, leftItemIndexShifter);

                RightItemContainer<TLeftItem, TRightItem, TComparablePart> createRightItemContainer()
                    => new RightItemContainer<TLeftItem, TRightItem, TComparablePart>(rightItem, rightCommonValue, earlyRightValueIndex);

                if (hasLeftItem && hasRightItem) {
                    var leftItemContainer = createLeftItemContainer();
                    var rightItemContainer = createRightItemContainer();

                    // Check if: "Both items are equal" => (true supports [1,..] <-> [1,..])
                    //       AND "Left and right buckets are empty"  (false supports [1,1,2,..] <-> [2,1,..])
                    if (equalityComparer.Equals(leftCommonValue!, rightCommonValue!)
                        && ((lateRightItemContainers.Buckets.TryGetValue(rightItemContainer).Item2?.All(x => x.CachedLeftItem != null)) ?? false)) {
                        syncedIterationModification = new CollectionModification<TRightItem, TLeftItem>(
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
                        syncedIterationModification = CollectionModification<TRightItem, TLeftItem>.CreateOld(NotifyCollectionChangedAction.Remove, leftItem, newLeftValueIndex);
                    } else {
                        lateLeftItemContainers.AddLast(createLeftItemContainer());
                    }

                    earlyLeftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual) {
                        syncedIterationModification = CollectionModification<TRightItem, TLeftItem>.CreateNew(NotifyCollectionChangedAction.Add, rightItem, earlyRightValueIndex);
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
                LeftItemContainer<TLeftItem, TRightItem, TComparablePart>? foundLeftItemContainer;

                if (hasCachedLeftItem) {
                    foundLeftItemContainer = rightItemContainer.CachedLeftItem;
                } else {
                    if (lateLeftItemContainers.TryGetBucket(CommonValueContainer<TComparablePart>.CreateEqualComparableItem(rightValue), out var bucket) && bucket.Count != 0) {
                        var firstNode = bucket.First!;
                        foundLeftItemContainer = firstNode.Value;
                        lateLeftItemContainers.Remove(firstNode);
                    } else {
                        foundLeftItemContainer = null;
                    }
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                if (foundLeftItemContainer == null) {
                    var modification = CollectionModification<TRightItem, TLeftItem>.CreateNew(NotifyCollectionChangedAction.Add, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
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

                        var modification = new CollectionModification<TRightItem, TLeftItem>(NotifyCollectionChangedAction.Move, foundLeftItemContainer.LeftItem, foundLeftIndex, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                        // We move the old existing item
                        yield return modification;
                        leftItemIndexShifter.DispatchObject(modification);
                    }

                    if (!hasCachedLeftItem) {
                        // Then we replace the left item by moved item at the destination index of the moved item
                        yield return new CollectionModification<TRightItem, TLeftItem>(NotifyCollectionChangedAction.Replace, foundLeftItemContainer.LeftItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightItemContainer.RightItem, rightValueIndex);
                    }
                }
            }

            if (!(lateLeftItemContainers.Last is null)) {
                // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
                foreach (var leftValueIndexPair in lateLeftItemContainers.Last.ListPart.GetEnumerableButReversed()) {
                    yield return CollectionModification<TRightItem, TLeftItem>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value.LeftItem, leftValueIndexPair.Value.ShiftedIndex);
                }
            }
        }

        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="TLeftItem">The type of left items.</typeparam>
        /// <typeparam name="TRightItem">The type of right items.</typeparam>
        /// <typeparam name="TComparablePart">The type of the comparable part of left item and right item.</typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="getComparablePartOfLeftItem">The part of left item that is comparable with part of right item.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="getComparablePartOfRightItem">The part of right item that is comparable with part of left item.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem, TComparablePart>(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem> rightItems,
            Func<TLeftItem, TComparablePart> getComparablePartOfLeftItem,
            Func<TRightItem, TComparablePart> getComparablePartOfRightItem) =>
            YieldCollectionModifications(
                leftItems,
                getComparablePartOfLeftItem,
                rightItems,
                getComparablePartOfRightItem,
                EqualityComparer<TComparablePart>.Default);


        /// <summary>
        /// The algorithm creates modifications that can transform one collection into another collection.
        /// The collection modifications may be used to transform <paramref name="leftItems"/>.
        /// The collection is not assumed to be in any order.
        /// Sorted duplications are allowed.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <param name="equalityComparer"></param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem> rightItems,
            IEqualityComparer<TItem> equalityComparer) =>
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
        /// <typeparam name="TItem"></typeparam>
        /// <param name="leftItems">The collection you want to have transformed.</param>
        /// <param name="rightItems">The collection in which <paramref name="leftItems"/> could be transformed.</param>
        /// <returns>For <paramref name="leftItems"/> the collection modifications between <paramref name="leftItems"/> and <paramref name="rightItems"/></returns>
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem> rightItems) =>
            YieldCollectionModifications(
                leftItems,
                leftItem => leftItem,
                rightItems,
                rightItem => rightItem,
                EqualityComparer<TItem>.Default);

        private class LeftItemContainer<TLeftItem, TRightItem, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            [AllowNull, MaybeNull]
            public TLeftItem LeftItem { get; private set; }

            public LeftItemContainer([AllowNull] TLeftItem leftItem, [AllowNull] CommonValueType commonValue, int index,
                ObjectEventDispatcher<CollectionModification<TRightItem, TLeftItem>> indexShifter)
                : base(commonValue, index)
            {
                LeftItem = leftItem;
                indexShifter = indexShifter ?? throw new ArgumentNullException(nameof(indexShifter));
                indexShifter.ObjectDispatch += IndexShifter_ObjectDispatch;
            }

            protected void IndexShifter_ObjectDispatch(object? sender, ObjectDispachEventArgs<CollectionModification<TRightItem, TLeftItem>> args)
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

        private class RightItemContainer<TLeftItem, TRightItem, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            [AllowNull]
            [MaybeNull]
            public TRightItem RightItem { get; private set; }
            public LeftItemContainer<TLeftItem, TRightItem, CommonValueType>? CachedLeftItem { get; set; }

            public RightItemContainer([AllowNull] TRightItem rightItem, [AllowNull] CommonValueType commonValue, int index)
                : base(commonValue, index)
                => RightItem = rightItem;
        }

        [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
        private class CommonValueContainer<CommonValueType>
        {
            public static CommonValueContainer<CommonValueType> CreateEqualComparableItem([AllowNull] CommonValueType value)
                => new CommonValueContainer<CommonValueType>(value, -1);

            [AllowNull]
            [MaybeNull]
            public CommonValueType CommonValue { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            protected CommonValueContainer([AllowNull] CommonValueType commonValue, int index)
            {
                CommonValue = commonValue;
                InitialIndex = index;
            }

            private string GetDebuggerDisplay() =>
                $"[{CommonValue}, {ShiftedIndex}]";
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

