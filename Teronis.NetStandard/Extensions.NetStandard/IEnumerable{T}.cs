using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections;
using Teronis.Collections.Generic;
using Teronis.Extensions.NetStandard;
using Teronis.Collections.ObjectModel;
using Teronis.Diagnostics;
using System.Diagnostics;
using Teronis.Libraries.NetStandard;
using System.Threading.Tasks;
using System.Collections;

namespace Teronis.Extensions.NetStandard
{
    public static class IEnumerableGenericExtensions
    {
        public static IEnumerable<T> ExcludeNulls<T>(this IEnumerable<T> collection) where T : class => collection.Where(x => x != null);

        public static R FirstNonDefaultOrDefault<T, R>(this IEnumerable<T> collection, Func<T, R> getObj)
        {
            foreach (var item in collection) {
                var obj = getObj(item);

                if (obj != default)
                    return obj;
            }

            return default;
        }

        public static R FirstNonDefault<T, R>(this IEnumerable<T> collection, Func<T, R> getObj) where R : class
        {
            var obj = FirstNonDefaultOrDefault(collection, getObj);

            if (obj != default)
                return obj;
            else
                throw new InvalidOperationException("The source sequence is empty.");
        }

        public static bool Any<T>(this IEnumerable<T> collection, Func<T, bool> predicate, out T last)
        {
            T _last = default;
            var retVal = collection.Any(x => predicate(_last = x));
            last = _last;
            return retVal;
        }

        public static IEnumerable<ValueIndexPair<T>> WithIndex<T>(this IEnumerable<T> sequence)
        {
            int i = 0;

            foreach (var value in sequence) {
                yield return new ValueIndexPair<T>(value, i);
                i++;
            }
        }

        /// <summary>
        /// Adds the item at the end of a new <see cref="IEnumerable{T}"/> sequence without loosing the target items.
        /// </summary>
        public static IEnumerable<T> ContinueWith<T>(this IEnumerable<T> target, T item)
        {
            if (target == null)
                throw new ArgumentException(nameof(target));

            foreach (var t in target)
                yield return t;

            yield return item;
        }

        /// <summary>
        /// Adds the items at the end of a new <see cref="IEnumerable{T}"/> sequence without loosing the target items.
        /// </summary>
        public static IEnumerable<T> ContinueWith<T>(this IEnumerable<T> target, IEnumerable<T> items)
        {
            if (target == null)
                throw new ArgumentException(nameof(target));

            foreach (var t in target)
                yield return t;

            foreach (var t in items)
                yield return t;
        }

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
            var leftItemsEnumerator = leftItems.GetEnumerator();
            var rightItemsEnumerator = rigthItems.GetEnumerator();
            var hasLeftItem = true;
            var hasRightItem = true;
            var earlyEqualIterationCount = 0;
            var earlyLeftValueIndex = 0;
            var earlyRightValueIndex = 0;
            // Early changes that are synchronized or are synchronized but having
            // removing or adding changes at the end can be returned all at once.
            // This list represents the cache for it.
            var earlyIterationChanges = new List<CollectionChange<LeftItemType, RightItemType>>();
            var leftValueIndexShifter = new IndexShifter<CollectionChange<LeftItemType, RightItemType>>();
            var lateLeftValueIndexPairs = new OrderedHashSet<ItemContainer<LeftItemType>, LeftItemContainer<LeftItemType, RightItemType>>();
            var lateRightValueIndexPairs = new OrderedHashSet<RightItemContainer<LeftItemType, RightItemType>>();
            var areEarlyIterationValuesEqual = true;

            bool getHasValue(ref bool hasItem, IEnumerator itemEnumerator)
                => hasItem && (hasItem = itemEnumerator.MoveNext());

            while (getHasValue(ref hasLeftItem, leftItemsEnumerator) | getHasValue(ref hasRightItem, rightItemsEnumerator)) {
                // Cancel if not a left and right value is available
                if (!(hasLeftItem || hasRightItem))
                    break;

                var leftItem = hasLeftItem ? leftItemsEnumerator.Current : default;
                var leftItemCommonValue = hasLeftItem ? getCommonValueFromLeftItem(leftItem) : default;

                var rightItem = hasRightItem ? rightItemsEnumerator.Current : default;
                var rightItemCommonValue = hasRightItem ? getCommonValueFromRightItem(rightItem) : default;
                CollectionChange<LeftItemType, RightItemType> earlyIterationChange = default;

                LeftItemContainer<LeftItemType, RightItemType> createLeftItemContainer() => new LeftItemContainer<LeftItemType, RightItemType>(leftItem, earlyLeftValueIndex, leftValueIndexShifter);
                RightItemContainer<LeftItemType, RightItemType> createRightItemContainer() => new RightItemContainer<LeftItemType, RightItemType>(rightItem, earlyRightValueIndex);

                if (hasLeftItem && hasRightItem) {
                    var areBothItemsEqual = commonValueEqualityComparer.Equals(leftItemCommonValue, rightItemCommonValue);

                    if (areBothItemsEqual) {
                        earlyIterationChange = new CollectionChange<LeftItemType, RightItemType>(NotifyCollectionChangedAction.Replace, leftItem, earlyLeftValueIndex, rightItem, earlyRightValueIndex);

                        // We only move forward to back, but never back to forward, so this item, even when 
                        // it's now well positioned, needs to be checked, if it is still well positioned.
                        var rightItemContainer = createRightItemContainer();
                        // We take use of a cache, so that we don't have to search for left item and don't replace it twice
                        rightItemContainer.CachedLeftItem = createLeftItemContainer();
                        lateRightValueIndexPairs.Add(rightItemContainer);
                    } else {
                        lateLeftValueIndexPairs.Add(createLeftItemContainer());
                        lateRightValueIndexPairs.Add(createRightItemContainer());
                        areEarlyIterationValuesEqual = false;
                    }

                    earlyEqualIterationCount++;
                    earlyLeftValueIndex = earlyEqualIterationCount;
                    earlyRightValueIndex = earlyEqualIterationCount;
                } else if (hasLeftItem) {
                    if (areEarlyIterationValuesEqual) {
                        // When early iterations are synchronized, then we always need 
                        // to delete the left value at index of the greatest right value index plus one
                        var newLeftValueIndex = earlyLeftValueIndex - (earlyLeftValueIndex - earlyRightValueIndex);
                        earlyIterationChange = CollectionChange<LeftItemType, RightItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftItem, newLeftValueIndex);
                    } else
                        lateLeftValueIndexPairs.Add(createLeftItemContainer());

                    earlyLeftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        earlyIterationChange = CollectionChange<LeftItemType, RightItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightItem, earlyRightValueIndex);
                    else
                        lateRightValueIndexPairs.Add(createRightItemContainer());

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

            foreach (var rightValueIndexPair in lateRightValueIndexPairs) {
                // This index represents the current index of the right value collection.
                var rightValueIndex = rightValueIndexPair.ShiftedIndex;
                var rightValue = rightValueIndexPair.Value;
                var hasCachedLeftItem = rightValueIndexPair.CachedLeftItem != null;
                LeftItemContainer<LeftItemType, RightItemType> foundLeftValueIndexPair = null;

                if (hasCachedLeftItem)
                    foundLeftValueIndexPair = rightValueIndexPair.CachedLeftItem;
                else {
                    if (lateLeftValueIndexPairs.TryGetValue(ItemContainer<LeftItemType>.CreateEqualComparableItem(rightValue), out foundLeftValueIndexPair))
                        lateLeftValueIndexPairs.Remove(foundLeftValueIndexPair);
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                if (foundLeftValueIndexPair == null) {
                    var change = CollectionChange<ItemType>.CreateNew(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                    yield return change;
                    leftValueIndexShifter.Shift(change);
                } else {
                    var foundLeftIndex = foundLeftValueIndexPair.ShiftedIndex;

                    // Indexes can be equal, when not processed items are before the moved item
                    if (foundLeftIndex != rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount) {
                        // Here we deny the forward to backward move, so we have to process already replaced items, 
                        // and move them forward, because they could be now behind those skipped items, but need to
                        // be moved before them
                        if (foundLeftIndex < rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount)
                            continue;

                        var change = new CollectionChange<ItemType>(NotifyCollectionChangedAction.Move, foundLeftValueIndexPair.Value, foundLeftIndex, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                        // We move the old existing item
                        yield return change;
                        leftValueIndexShifter.Shift(change);
                    }

                    if (!hasCachedLeftItem)
                        // Then we replace the left item by moved item at the destination index of the moved item
                        yield return new CollectionChange<ItemType>(NotifyCollectionChangedAction.Replace, foundLeftValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightValueIndexPair.Value, rightValueIndex);
                }
            }

            // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
            foreach (var leftValueIndexPair in lateLeftValueIndexPairs.YieldReversedItems())
                yield return CollectionChange<ItemType>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.ShiftedIndex);
        }

        //public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
        //    => GetCollectionChanges(leftValues, rightValues, default);

        //public static Task<List<CollectionChange<T>>> GetCollectionChangesAsync<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        //    => Task.Run(() => GetCollectionChanges(leftValues, rightValues, equalityComparer).ToList());

        //public static Task<List<CollectionChange<T>>> GetCollectionChangesAsync<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
        //    => GetCollectionChangesAsync(leftValues, rightValues, default);

        private class LeftItemContainer<LeftItemType, RightItemType> : ItemContainer<LeftItemType>
        {
            public LeftItemContainer(LeftItemType value, int index, IndexShifter<CollectionChange<LeftItemType, RightItemType>> shifter)
                : base(value, index)
            {
                shifter = shifter ?? throw new ArgumentNullException(nameof(shifter));
                shifter.IndexShiftConditionEvaluating += Shifter_IndexShiftConditionEvaluating;
            }

            protected void Shifter_IndexShiftConditionEvaluating(object sender, IndexShiftConditionEvaluatingEventArgs<CollectionChange<LeftItemType, RightItemType>> args)
            {
                var change = args.ShiftCondition;

                switch (args.ShiftCondition.Action) {
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

        private class RightItemContainer<LeftItemType, RightItemType> : ItemContainer<RightItemType>
        {
            public LeftItemContainer<LeftItemType, RightItemType> CachedLeftItem { get; set; }

            public RightItemContainer(RightItemType value, int index)
                : base(value, index) { }
        }

        [DebuggerDisplay(ObjectLibrary.FullToDebugStringMethodPathWithParameterizedThis)]
        private class ItemContainer<TValue> : IDebuggerDisplay
        {
            public static ItemContainer<TValue> CreateEqualComparableItem(TValue value) => new ItemContainer<TValue>(value, -1);

            public TValue Value { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            string IDebuggerDisplay.DebuggerDisplay => $"[{Value}, {ShiftedIndex}]";

            protected ItemContainer(TValue value, int index)
            {
                Value = value;
                InitialIndex = index;
            }
        }

        private class ItemEqualityComparer<TValue> : IEqualityComparer<ItemContainer<TValue>>
        {
            public IEqualityComparer<TValue> ValueEqualityComparer { get; private set; }

            public ItemEqualityComparer(IEqualityComparer<TValue> valueEqualityComparer)
                => ValueEqualityComparer = ValueEqualityComparer ?? throw new ArgumentNullException(nameof(valueEqualityComparer));

            public bool Equals(ItemContainer<TValue> x, ItemContainer<TValue> y)
                => ValueEqualityComparer.Equals(x.Value, y.Value);

            public int GetHashCode(ItemContainer<TValue> obj)
                => ValueEqualityComparer.GetHashCode(obj.Value);
        }
    }
}
