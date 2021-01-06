using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Collections.Changes;
using Teronis.Collections.ObjectModel;
using Teronis.Collections.Specialized;
using Teronis.Diagnostics;

namespace Teronis.Extensions
{
    public static partial class IEnumerableGenericExtensions
    {
        /// <summary>
        /// Get step by step modifications to reorder the left value collection, without to need to clear it.
        /// The modifications can contain adding, replacing and removing statements, but nothing else.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="leftItems"></param>
        /// <param name="rigthItems"></param>
        /// <param name="commonValueEqualityComparer"></param>
        /// <returns></returns>
        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> GetCollectionModifications<LeftItemType, RightItemType, CommonValueType>(
            this IEnumerable<LeftItemType> leftItems,
            IEnumerable<RightItemType> rigthItems,
            Func<LeftItemType, CommonValueType> getCommonValueFromLeftItem,
            Func<RightItemType, CommonValueType> getCommonValueFromRightItem,
            IEqualityComparer<CommonValueType> commonValueEqualityComparer)
        {
            commonValueEqualityComparer ??= EqualityComparer<CommonValueType>.Default;
            var commonValueContainerEqualityComparer = new CommonValueContainerEqualityComparer<CommonValueType>(commonValueEqualityComparer);
            var leftItemEnumerator = leftItems.GetEnumerator();
            var rightItemEnumerator = rigthItems.GetEnumerator();
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
            ILinkedBucketList<CommonValueContainer<CommonValueType>, LeftItemContainer<LeftItemType, RightItemType, CommonValueType>> lateLeftItemContainers = new LinkedBucketList<CommonValueContainer<CommonValueType>, LeftItemContainer<LeftItemType, RightItemType, CommonValueType>>(commonValueContainerEqualityComparer);
            var lateRightItemContainers = new LinkedBucketList<CommonValueContainer<CommonValueType>, RightItemContainer<LeftItemType, RightItemType, CommonValueType>>(commonValueContainerEqualityComparer);
            var areEarlyIterationValuesEqual = true;

            /* Cache left and right items */
            while ((hasLeftItem && (hasLeftItem = leftItemEnumerator.MoveNext()))
                | (hasRightItem && (hasRightItem = rightItemEnumerator.MoveNext()))) {
                // Cancel if not a left and right value is available
                if (!(hasLeftItem || hasRightItem)) {
                    break;
                }

                var leftItem = hasLeftItem ? leftItemEnumerator.Current : default;
                var leftCommonValue = hasLeftItem ? getCommonValueFromLeftItem(leftItem!) : default;

                var rightItem = hasRightItem ? rightItemEnumerator.Current : default;
                var rightCommonValue = hasRightItem ? getCommonValueFromRightItem(rightItem!) : default;
                CollectionModification<LeftItemType, RightItemType>? syncedIterationModification = default;

                LeftItemContainer<LeftItemType, RightItemType, CommonValueType> createLeftItemContainer()
                    => new LeftItemContainer<LeftItemType, RightItemType, CommonValueType>(leftItem, leftCommonValue, earlyLeftValueIndex, leftItemIndexShifter);

                RightItemContainer<LeftItemType, RightItemType, CommonValueType> createRightItemContainer()
                    => new RightItemContainer<LeftItemType, RightItemType, CommonValueType>(rightItem, rightCommonValue, earlyRightValueIndex);

                if (hasLeftItem && hasRightItem) {
                    var leftItemContainer = createLeftItemContainer();
                    var rightItemContainer = createRightItemContainer();

                    // Check if: "Both items are equal" => (true supports [1,..] <-> [1,..])
                    //       AND "Left and right buckets are empty"  (false supports [1,1,2,..] <-> [2,1,..])
                    if (commonValueEqualityComparer.Equals(leftCommonValue!, rightCommonValue!)
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
                LeftItemContainer<LeftItemType, RightItemType, CommonValueType>? foundLeftItemContainer;

                if (hasCachedLeftItem) {
                    foundLeftItemContainer = rightItemContainer.CachedLeftItem;
                } else {
                    if (lateLeftItemContainers.TryGetBucket(CommonValueContainer<CommonValueType>.CreateEqualComparableItem(rightValue), out var bucket) && bucket.Count != 0) {
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

        public static IEnumerable<CollectionModification<LeftItemType, RightItemType>> GetCollectionModifications<LeftItemType, RightItemType>(
            this IEnumerable<LeftItemType> leftItems, 
            IEnumerable<RightItemType> rightItems, 
            Func<LeftItemType, RightItemType> getCommonValueFromLeftItem, 
            IEqualityComparer<RightItemType> commonValueEqualityComparer)
        {
            static RightItemType getCommonValueFromRightItem(RightItemType rightItem) =>
                rightItem;

            return GetCollectionModifications(leftItems, rightItems, getCommonValueFromLeftItem, getCommonValueFromRightItem, commonValueEqualityComparer);
        }

        public static IEnumerable<CollectionModification<ItemType, ItemType>> GetCollectionModifications<ItemType>(
            this IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType> rightItems,
            IEqualityComparer<ItemType> commonValueEqualityComparer)
        {
            static ItemType getCommonValueFromItem(ItemType item) => item;
            return GetCollectionModifications(leftItems, rightItems, getCommonValueFromItem, getCommonValueFromItem, commonValueEqualityComparer);
        }

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
