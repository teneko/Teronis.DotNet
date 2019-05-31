using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections;
using Teronis.Collections.Generic;
using Teronis.Extensions.NetStandard;
using Teronis.Collections.ObjectModel;

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
        /// Add / Replace / Remove
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
            // removing or adding changes at the end can be returned immediately.
            // This list represents the cache for that.
            var earlyIterationChanges = new List<CollectionChange<T>>();
            var leftValueIndexShifter = new IndexShifter<CollectionChange<T>>();
            var lateLeftValueIndexPairs = new List<LeftItem<T>>();
            var lateRightValueIndexPairs = new List<Item<T>>();
            var areEarlyIterationValuesEqual = true;

            do {
                if (hasLeftValue)
                    hasLeftValue = leftValuesEnumerator.MoveNext();

                if (hasRightValue)
                    hasRightValue = rightValuesEnumerator.MoveNext();

                // Cancel if not a left or right value is available
                if (!(hasLeftValue || hasRightValue))
                    break;

                T leftValue;
                T rightValue;

                if (hasLeftValue)
                    leftValue = leftValuesEnumerator.Current;
                else
                    leftValue = default; // Only for compiler

                if (hasRightValue)
                    rightValue = rightValuesEnumerator.Current;
                else
                    rightValue = default; // Only for compiler

                CollectionChange<T> change = default;

                if (hasLeftValue && hasRightValue) {
                    var areBothItemsEqual = equalityComparer.Equals(leftValue, rightValue);

                    if (areBothItemsEqual)
                        change = new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, earlyLeftValueIndex, rightValue, earlyRightValueIndex);
                    else {
                        lateLeftValueIndexPairs.Add(new LeftItem<T>(leftValue, earlyLeftValueIndex, leftValueIndexShifter));
                        lateRightValueIndexPairs.Add(new Item<T>(rightValue, earlyRightValueIndex));
                        areEarlyIterationValuesEqual = false;
                    }

                    earlyEqualIterationCount++;
                    earlyLeftValueIndex = earlyEqualIterationCount;
                    earlyRightValueIndex = earlyEqualIterationCount;
                } else if (hasLeftValue) {
                    if (areEarlyIterationValuesEqual) {
                        var newLeftValueIndex = earlyLeftValueIndex - (earlyLeftValueIndex - earlyRightValueIndex);
                        change = CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValue, newLeftValueIndex);
                    } else
                        lateLeftValueIndexPairs.Add(new LeftItem<T>(leftValue, earlyLeftValueIndex, leftValueIndexShifter));

                    earlyLeftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        change = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValue, earlyRightValueIndex);
                    else
                        lateRightValueIndexPairs.Add(new Item<T>(rightValue, earlyRightValueIndex));

                    earlyRightValueIndex++;
                }

                if (change != null)
                    earlyIterationChanges.Add(change);
            } while (true);

            foreach (var change in earlyIterationChanges)
                yield return change;

            // Exit only if both lists were synchronized from early on
            if (areEarlyIterationValuesEqual)
                yield break;

            for (var rightValueIndexPairIndex = 0; rightValueIndexPairIndex < lateRightValueIndexPairs.Count; rightValueIndexPairIndex++) {
                var rightValueIndexPair = lateRightValueIndexPairs[rightValueIndexPairIndex];
                // This index represents the current index of the right value collection.
                var rightValueIndex = rightValueIndexPair.ShiftedIndex;
                Item<T> foundLeftValueIndexPair = null;

                for (var leftValueIndexPairIndex = 0; leftValueIndexPairIndex < lateLeftValueIndexPairs.Count; leftValueIndexPairIndex++) {
                    var leftValueIndexPair = lateLeftValueIndexPairs[leftValueIndexPairIndex];

                    if (equalityComparer.Equals(leftValueIndexPair.Value, rightValueIndexPair.Value)) {
                        foundLeftValueIndexPair = leftValueIndexPair;
                        // We remove the found left item, because we are sure that we don't need that reference anymore
                        lateLeftValueIndexPairs.RemoveAt(leftValueIndexPairIndex);
                        // We found a left value that is equal to the right value and due that fact, we cancel the search
                        break;
                    }
                }

                var rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount = rightValueIndex;

                // We assume that late-left-value-index-pairs it's indexes in collection are sorted ascending
                foreach (var leftValueIndexPair in lateLeftValueIndexPairs) {
                    if (leftValueIndexPair.ShiftedIndex < rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount)
                        rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount++;
                }

                if (foundLeftValueIndexPair == null) {
                    var change = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);
                    yield return change;
                    leftValueIndexShifter.Shift(change);
                } else {
                    // Indexes can be equal, when not processed items are before the moved item
                    if (foundLeftValueIndexPair.ShiftedIndex != rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount) {
                        var foundLeftValueIndex = foundLeftValueIndexPair.ShiftedIndex;

                        // We move the old existing item
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Move, foundLeftValueIndexPair.Value, foundLeftValueIndex, rightValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount);

                        // Now we have to increase the index by one from those items that are between the old index and new index + 1 of the moved item,
                        foreach (var leftValueIndexPair in lateLeftValueIndexPairs) {
                            var leftValueIndexPairIndex = leftValueIndexPair.ShiftedIndex;

                            if (leftValueIndexPairIndex >= rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount && leftValueIndexPairIndex < foundLeftValueIndex)
                                leftValueIndexPair.Shifts++;
                        }
                    }

                    // Then we replace the left item by moved item at the destination index of the moved item
                    yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, foundLeftValueIndexPair.Value, rightValueIndexWithNotProcessedItemsBeforeRightValueIndexCount, rightValueIndexPair.Value, rightValueIndex);
                }
            }

            // We remove all left left-value-index-pairs, because they did not match any condition above and have to be removed in REVERSED order
            for (var leftValueIndexPairIndex = lateLeftValueIndexPairs.Count - 1; leftValueIndexPairIndex >= 0; leftValueIndexPairIndex--) {
                var leftValueIndexPair = lateLeftValueIndexPairs[leftValueIndexPairIndex];
                yield return CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.ShiftedIndex);
            }
        }

        public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionChanges(leftValues, rightValues, EqualityComparer<T>.Default);

        private class LeftItem<TValue> : Item<TValue>
        {
            public LeftItem(TValue value, int index, IndexShifter<CollectionChange<TValue>> shifter)
                : base(value, index, shifter) { }

            protected override void Shifter_IndexShiftConditionEvaluating(object sender, IndexShiftConditionEvaluatingEventArgs<CollectionChange<TValue>> args)
            {
                var change = args.ShiftCondition;

                switch (args.ShiftCondition.Action) {
                    case NotifyCollectionChangedAction.Add:
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

        private class Item<TValue>
        {
            public TValue Value { get; set; }
            public int InitialIndex { get; set; }
            public int Shifts { get; protected set; }
            public int ShiftedIndex => InitialIndex + Shifts;

            protected Item(TValue value, int index, IndexShifter<CollectionChange<TValue>> shifter)
            {
                Value = value;
                InitialIndex = index;

                if (shifter != null)
                    shifter.IndexShiftConditionEvaluating += Shifter_IndexShiftConditionEvaluating;
            }

            public Item(TValue value, int index)
                : this(value, index, default) { }

            protected virtual void Shifter_IndexShiftConditionEvaluating(object sender, IndexShiftConditionEvaluatingEventArgs<CollectionChange<TValue>> args)
            { }

            public override string ToString() => $"[{Value}, {ShiftedIndex}]";
        }
    }
}
