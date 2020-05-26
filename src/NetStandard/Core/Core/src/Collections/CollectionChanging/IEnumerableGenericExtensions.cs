using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Generic;
using Teronis.Collections.ObjectModel;
using Teronis.Diagnostics;
using System.Diagnostics;
using System.Collections;
using Teronis.Collections.CollectionChanging;

namespace Teronis.Extensions
{
    public static partial class IEnumerableGenericExtensions
    {
        /// <summary>
        /// Get step by step changes to reorder the original left value collection, without to need to clear it.
        /// The changes can contain adding, replacing and removing statements, but nothing else.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="leftItems"></param>
        /// <param name="rigthItems"></param>
        /// <param name="commonValueEqualityComparer"></param>
        /// <returns></returns>
        public static IEnumerable<CollectionChange<LeftItemType, RightItemType>> GetCollectionChanges<LeftItemType, RightItemType, CommonValueType>(this IEnumerable<LeftItemType> leftItems, IEnumerable<RightItemType> rigthItems, Func<LeftItemType, CommonValueType> getCommonValueFromLeftItem, Func<RightItemType, CommonValueType> getCommonValueFromRightItem, IEqualityComparer<CommonValueType> commonValueEqualityComparer)
        {
            commonValueEqualityComparer = commonValueEqualityComparer ?? EqualityComparer<CommonValueType>.Default;
            var commonValueContainerEqualityComparer = new CommonValueContainerEqualityComparer<CommonValueType>(commonValueEqualityComparer);
            var leftItemsEnumerator = leftItems.GetEnumerator();
            var rightItemsEnumerator = rigthItems.GetEnumerator();
            var hasLeftItem = true;
            var hasRightItem = true;
            // It will only count until a left or right item is not found
            var earlyIterationCount = 0;
            var earlyLeftValueIndex = 0;
            var earlyRightValueIndex = 0;
            // Early changes that are synchronized or are synchronized but having
            // removing or adding changes at the end can be returned all at once.
            // This list represents the cache for it.
            var earlyIterationChanges = new List<CollectionChange<LeftItemType, RightItemType>>();
            var leftItemIndexShifter = new IndexShifter<CollectionChange<LeftItemType, RightItemType>>();
            var lateLeftItemContainers = new OrderedHashSet<CommonValueContainer<CommonValueType>, LeftItemContainer<LeftItemType, RightItemType, CommonValueType>>(commonValueContainerEqualityComparer);
            var lateRightItemContainers = new OrderedHashSet<CommonValueContainer<CommonValueType>, RightItemContainer<LeftItemType, RightItemType, CommonValueType>>(commonValueContainerEqualityComparer);
            var areEarlyIterationValuesEqual = true;

            bool getHasValue(ref bool hasItem, IEnumerator itemEnumerator)
                => hasItem && (hasItem = itemEnumerator.MoveNext());

            while (getHasValue(ref hasLeftItem, leftItemsEnumerator) | getHasValue(ref hasRightItem, rightItemsEnumerator))
            {
                // Cancel if not a left and right value is available
                if (!(hasLeftItem || hasRightItem))
                    break;

                var leftItem = hasLeftItem ? leftItemsEnumerator.Current : default;
                var leftCommonValue = hasLeftItem ? getCommonValueFromLeftItem(leftItem) : default;

                var rightItem = hasRightItem ? rightItemsEnumerator.Current : default;
                var rightCommonValue = hasRightItem ? getCommonValueFromRightItem(rightItem) : default;
                CollectionChange<LeftItemType, RightItemType> earlyIterationChange = default;

                LeftItemContainer<LeftItemType, RightItemType, CommonValueType> createLeftItemContainer()
                    => new LeftItemContainer<LeftItemType, RightItemType, CommonValueType>(leftItem, leftCommonValue, earlyLeftValueIndex, leftItemIndexShifter);

                RightItemContainer<LeftItemType, RightItemType, CommonValueType> createRightItemContainer()
                    => new RightItemContainer<LeftItemType, RightItemType, CommonValueType>(rightItem, rightCommonValue, earlyRightValueIndex);

                if (hasLeftItem && hasRightItem)
                {
                    var areBothItemsEqual = commonValueEqualityComparer.Equals(leftCommonValue, rightCommonValue);

                    if (areBothItemsEqual)
                    {
                        earlyIterationChange = new CollectionChange<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Replace, leftItem, earlyLeftValueIndex, rightItem, earlyRightValueIndex);

                        // We only move forward to back, but never back to forward, so this item, even when 
                        // it's now well positioned, needs to be checked, if it is still well positioned.
                        var rightItemContainer = createRightItemContainer();
                        // We take use of a cache, so that we don't have to search for left item and don't replace it twice
                        rightItemContainer.CachedLeftItem = createLeftItemContainer();
                        lateRightItemContainers.Add(rightItemContainer);
                    }
                    else
                    {
                        lateLeftItemContainers.Add(createLeftItemContainer());
                        lateRightItemContainers.Add(createRightItemContainer());
                        areEarlyIterationValuesEqual = false;
                    }

                    earlyIterationCount++;
                    earlyLeftValueIndex = earlyIterationCount;
                    earlyRightValueIndex = earlyIterationCount;
                }
                else if (hasLeftItem)
                {
                    if (areEarlyIterationValuesEqual)
                    {
                        // When early iterations are synchronized, then we always need 
                        // to delete the left value at index of the greatest right value index plus one
                        var newLeftValueIndex = earlyLeftValueIndex - (earlyLeftValueIndex - earlyRightValueIndex);
                        earlyIterationChange = CollectionChange<LeftItemType, RightItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftItem, newLeftValueIndex);
                    }
                    else
                        lateLeftItemContainers.Add(createLeftItemContainer());

                    earlyLeftValueIndex++;
                }
                else
                {
                    if (areEarlyIterationValuesEqual)
                        earlyIterationChange = CollectionChange<LeftItemType, RightItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightItem, earlyRightValueIndex);
                    else
                        lateRightItemContainers.Add(createRightItemContainer());

                    earlyRightValueIndex++;
                }

                if (earlyIterationChange != null)
                    earlyIterationChanges.Add(earlyIterationChange);
            }

            foreach (var change in earlyIterationChanges)
                yield return change;

            // Exit only if both lists were synchronized from early on
            if (areEarlyIterationValuesEqual)
                yield break;

            foreach (var rightItemContainer in lateRightItemContainers)
            {
                // This index represents the current index of the right value collection.
                var rightValueIndex = rightItemContainer.ShiftedIndex;
                var rightValue = rightItemContainer.CommonValue;
                var hasCachedLeftItem = rightItemContainer.CachedLeftItem != null;
                LeftItemContainer<LeftItemType, RightItemType, CommonValueType> foundLeftValueIndexPair = null;

                if (hasCachedLeftItem)
                    foundLeftValueIndexPair = rightItemContainer.CachedLeftItem;
                else
                {
                    if (lateLeftItemContainers.TryGetValue(CommonValueContainer<CommonValueType>.CreateEqualComparableItem(rightValue), out foundLeftValueIndexPair))
                        lateLeftItemContainers.Remove(foundLeftValueIndexPair);
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                if (foundLeftValueIndexPair == null)
                {
                    var change = CollectionChange<LeftItemType, RightItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                    yield return change;
                    leftItemIndexShifter.Shift(change);
                }
                else
                {
                    var foundLeftIndex = foundLeftValueIndexPair.ShiftedIndex;

                    // Indexes can be equal, when not processed items are before the moved item
                    if (foundLeftIndex != rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount)
                    {
                        // Here we deny the forward to backward move, so we have to process already replaced items, 
                        // and move them forward, because they could be now behind those skipped items, but need to
                        // be moved before them
                        if (foundLeftIndex < rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount)
                            continue;

                        var change = new CollectionChange<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Move, foundLeftValueIndexPair.LeftItem, foundLeftIndex, rightItemContainer.RightItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                        // We move the old existing item
                        yield return change;
                        leftItemIndexShifter.Shift(change);
                    }

                    if (!hasCachedLeftItem)
                        // Then we replace the left item by moved item at the destination index of the moved item
                        yield return new CollectionChange<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Replace, foundLeftValueIndexPair.LeftItem, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightItemContainer.RightItem, rightValueIndex);
                }
            }

            // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
            foreach (var leftValueIndexPair in lateLeftItemContainers.YieldReversedItems())
                yield return CollectionChange<LeftItemType, RightItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.LeftItem, leftValueIndexPair.ShiftedIndex);
        }

        public static IEnumerable<CollectionChange<LeftItemType, RightItemType>> GetCollectionChanges<LeftItemType, RightItemType>(this IEnumerable<LeftItemType> leftItems, IEnumerable<RightItemType> rightItems, Func<LeftItemType, RightItemType> getCommonValueFromLeftItem, IEqualityComparer<RightItemType> commonValueEqualityComparer)
        {
            RightItemType getCommonValueFromRightItem(RightItemType rightItem)
                => rightItem;

            return GetCollectionChanges(leftItems, rightItems, getCommonValueFromLeftItem, getCommonValueFromRightItem, commonValueEqualityComparer);
        }

        public static IEnumerable<CollectionChange<ItemType, ItemType>> GetCollectionChanges<ItemType>(this IEnumerable<ItemType> leftItems, IEnumerable<ItemType> rightItems, IEqualityComparer<ItemType> commonValueEqualityComparer) {
            ItemType getCommonValueFromItem(ItemType item)
                => item;

            return GetCollectionChanges(leftItems, rightItems, getCommonValueFromItem, getCommonValueFromItem, commonValueEqualityComparer);
        }

        //public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
        //    => GetCollectionChanges(leftValues, rightValues, default);

        //public static Task<List<CollectionChange<T>>> GetCollectionChangesAsync<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        //    => Task.Run(() => GetCollectionChanges(leftValues, rightValues, equalityComparer).ToList());

        //public static Task<List<CollectionChange<T>>> GetCollectionChangesAsync<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
        //    => GetCollectionChangesAsync(leftValues, rightValues, default);

        private class LeftItemContainer<LeftItemType, RightItemType, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            public LeftItemType LeftItem { get; private set; }

            public LeftItemContainer(LeftItemType leftItem, CommonValueType commonValue, int index, IndexShifter<CollectionChange<LeftItemType, RightItemType>> shifter)
                : base(commonValue, index)
            {
                LeftItem = leftItem;
                shifter = shifter ?? throw new ArgumentNullException(nameof(shifter));
                shifter.IndexShiftConditionEvaluating += Shifter_IndexShiftConditionEvaluating;
            }

            protected void Shifter_IndexShiftConditionEvaluating(object sender, IndexShiftConditionEvaluatingEventArgs<CollectionChange<LeftItemType, RightItemType>> args)
            {
                var change = args.ShiftCondition;

                switch (args.ShiftCondition.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        // When adding a 
                        if (ShiftedIndex >= change.NewIndex)
                            Shifts++;

                        break;
                    case NotifyCollectionChangedAction.Move:
                        if (ShiftedIndex >= change.NewIndex && ShiftedIndex < change.OldIndex)
                            Shifts++;

                        break;
                }
            }
        }

        private class RightItemContainer<LeftItemType, RightItemType, CommonValueType> : CommonValueContainer<CommonValueType>
        {
            public RightItemType RightItem { get; private set; }
            public LeftItemContainer<LeftItemType, RightItemType, CommonValueType> CachedLeftItem { get; set; }

            public RightItemContainer(RightItemType rightItem, CommonValueType commonValue, int index)
                : base(commonValue, index)
                => RightItem = rightItem;
        }

        [DebuggerDisplay(IDebuggerDisplayLibrary.FullGetDebuggerDisplayMethodPathWithParameterizedThis)]
        private class CommonValueContainer<CommonValueType> : IDebuggerDisplay
        {
            public static CommonValueContainer<CommonValueType> CreateEqualComparableItem(CommonValueType value)
                => new CommonValueContainer<CommonValueType>(value, -1);

            public CommonValueType CommonValue { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            string IDebuggerDisplay.DebuggerDisplay => $"[{CommonValue}, {ShiftedIndex}]";

            protected CommonValueContainer(CommonValueType commonValue, int index)
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

            public bool Equals(CommonValueContainer<CommonValueType> x, CommonValueContainer<CommonValueType> y)
                => CommonValueEqualityComparer.Equals(x.CommonValue, y.CommonValue);

            public int GetHashCode(CommonValueContainer<CommonValueType> obj)
                => CommonValueEqualityComparer.GetHashCode(obj.CommonValue);
        }
    }
}
