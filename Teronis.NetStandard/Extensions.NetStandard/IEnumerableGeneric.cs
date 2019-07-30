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
        /// <typeparam name="T"></typeparam>
        /// <param name="leftValues"></param>
        /// <param name="rightValues"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        {
            var leftValuesEnumerator = leftValues.GetEnumerator();
            var rightValuesEnumerator = rightValues.GetEnumerator();
            var hasLeftValue = true;
            var hasRightValue = true;
            var earlyEqualIterationCount = 0;
            var earlyLeftValueIndex = 0;
            var earlyRightValueIndex = 0;
            // Early changes that are synchronized or are synchronized but having
            // removing or adding changes at the end can be returned all at once.
            // This list represents the cache for it.
            var earlyIterationChanges = new List<CollectionChange<T>>();
            var leftValueIndexShifter = new IndexShifter<CollectionChange<T>>();
            var lateLeftValueIndexPairs = new OrderedHashSet<Item<T>, LeftItem<T>>();
            var lateRightValueIndexPairs = new OrderedHashSet<RightItem<T>>();
            var areEarlyIterationValuesEqual = true;

            bool getHasValue(ref bool hasValue, IEnumerator<T> valueEnumerator)
                => hasValue && (hasValue = valueEnumerator.MoveNext());

            while (getHasValue(ref hasLeftValue, leftValuesEnumerator) | getHasValue(ref hasRightValue, rightValuesEnumerator)) {
                // Cancel if not a left and right value is available
                if (!(hasLeftValue || hasRightValue))
                    break;

                var leftValue = hasLeftValue ? leftValuesEnumerator.Current : default;
                var rightValue = hasRightValue ? rightValuesEnumerator.Current : default;
                CollectionChange<T> earlyIterationChange = default;

                LeftItem<T> createLeftItem() => new LeftItem<T>(leftValue, earlyLeftValueIndex, leftValueIndexShifter);
                RightItem<T> createRightItem() => new RightItem<T>(rightValue, earlyRightValueIndex);

                if (hasLeftValue && hasRightValue) {
                    var areBothItemsEqual = equalityComparer.Equals(leftValue, rightValue);

                    if (areBothItemsEqual) {
                        earlyIterationChange = new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, earlyLeftValueIndex, rightValue, earlyRightValueIndex);

                        // We only move back to forward, but never back to forward, so this item, even when it's now well positioned,
                        // needs to be checked, if it is still well positioned
                        var rightItem = createRightItem();
                        // We take us of a cache, so that we don't have to search for left item and don't replace it twice
                        rightItem.CachedLeftItem = createLeftItem();
                        lateRightValueIndexPairs.Add(rightItem);
                    } else {
                        lateLeftValueIndexPairs.Add(createLeftItem());
                        lateRightValueIndexPairs.Add(createRightItem());
                        areEarlyIterationValuesEqual = false;
                    }

                    earlyEqualIterationCount++;
                    earlyLeftValueIndex = earlyEqualIterationCount;
                    earlyRightValueIndex = earlyEqualIterationCount;
                } else if (hasLeftValue) {
                    if (areEarlyIterationValuesEqual) {
                        // When early iterations are synchronized, then we always need 
                        // to delete the left value at index of greatest right value index plus one
                        var newLeftValueIndex = earlyLeftValueIndex - (earlyLeftValueIndex - earlyRightValueIndex);
                        earlyIterationChange = CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValue, newLeftValueIndex);
                    } else
                        lateLeftValueIndexPairs.Add(createLeftItem());

                    earlyLeftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        earlyIterationChange = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValue, earlyRightValueIndex);
                    else
                        lateRightValueIndexPairs.Add(createRightItem());

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
                LeftItem<T> foundLeftValueIndexPair = null;

                if (hasCachedLeftItem)
                    foundLeftValueIndexPair = rightValueIndexPair.CachedLeftItem;
                else {
                    if (lateLeftValueIndexPairs.TryGetValue(Item<T>.CreateEqualComparableItem(rightValue), out foundLeftValueIndexPair))
                        lateLeftValueIndexPairs.Remove(foundLeftValueIndexPair);
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                if (foundLeftValueIndexPair == null) {
                    var change = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
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

                        var change = new CollectionChange<T>(NotifyCollectionChangedAction.Move, foundLeftValueIndexPair.Value, foundLeftIndex, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                        // We move the old existing item
                        yield return change;
                        leftValueIndexShifter.Shift(change);
                    }

                    if (!hasCachedLeftItem)
                        // Then we replace the left item by moved item at the destination index of the moved item
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, foundLeftValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightValueIndexPair.Value, rightValueIndex);
                }
            }

            // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
            foreach (var leftValueIndexPair in lateLeftValueIndexPairs.YieldReversedItems())
                yield return CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.ShiftedIndex);
        }

        public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionChanges(leftValues, rightValues, EqualityComparer<T>.Default);

        private class LeftItem<TValue> : Item<TValue>
        {
            public LeftItem(TValue value, int index, IndexShifter<CollectionChange<TValue>> shifter)
                : base(value, index)
            {
                shifter = shifter ?? throw new ArgumentNullException(nameof(shifter));
                shifter.IndexShiftConditionEvaluating += Shifter_IndexShiftConditionEvaluating;
            }

            protected void Shifter_IndexShiftConditionEvaluating(object sender, IndexShiftConditionEvaluatingEventArgs<CollectionChange<TValue>> args)
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

        private class RightItem<TValue> : Item<TValue>
        {
            public LeftItem<TValue> CachedLeftItem { get; set; }

            public RightItem(TValue value, int index)
                : base(value, index) { }
        }

        [DebuggerDisplay(ObjectLibrary.FullToDebugStringMethodPathWithParameterizedThis)]
        private class Item<TValue> : IDebuggerDisplay
        {
            public static Item<TValue> CreateEqualComparableItem(TValue value) => new Item<TValue>(value, -1);

            public TValue Value { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            string IDebuggerDisplay.DebuggerDisplay => $"[{Value}, {ShiftedIndex}]";

            protected Item(TValue value, int index)
            {
                Value = value;
                InitialIndex = index;
            }
        }

        private class ItemEqualityComparer<TValue> : IEqualityComparer<Item<TValue>>
        {
            public IEqualityComparer<TValue> ValueEqualityComparer { get; private set; }

            public ItemEqualityComparer(IEqualityComparer<TValue> valueEqualityComparer)
                => ValueEqualityComparer = ValueEqualityComparer ?? throw new ArgumentNullException(nameof(valueEqualityComparer));

            public bool Equals(Item<TValue> x, Item<TValue> y)
                => ValueEqualityComparer.Equals(x.Value, y.Value);

            public int GetHashCode(Item<TValue> obj)
                => ValueEqualityComparer.GetHashCode(obj.Value);
        }
    }
}
