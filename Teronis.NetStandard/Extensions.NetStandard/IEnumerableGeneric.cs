using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections;
using Teronis.Collections.Generic;
using Teronis.Extensions.NetStandard;

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
            var leftValueIndex = 0;
            var rightValueIndex = 0;
            var earlyIterationChanges = new List<CollectionChange<T>>();
            var leftValueIndexPairs = new List<ValueIndexPair<T>>();
            var rightValueIndexPairs = new List<ValueIndexPair<T>>();
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

                var change = default(CollectionChange<T>);

                if (hasLeftValue && hasRightValue) {
                    if (equalityComparer.Equals(leftValue, rightValue))
                        // Changes on the same index can always be returned immediately
                        change = new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, -1, rightValueIndex);
                    else {
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));
                        areEarlyIterationValuesEqual = false;
                    }

                    leftValueIndex++;
                    rightValueIndex++;
                } else if (hasLeftValue) {
                    if (areEarlyIterationValuesEqual) {
                        var newLeftValueIndex = leftValueIndex - (leftValueIndex - rightValueIndex);
                        change = CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValue, newLeftValueIndex);
                    } else
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));

                    leftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        change = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValue, rightValueIndex, rightValueIndex);
                    else
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));

                    rightValueIndex++;
                }

                if (change != null)
                    earlyIterationChanges.Add(change);
            } while (true);

            foreach (var change in earlyIterationChanges)
                yield return change;

            // Exit only if both lists were synced from early on
            if (areEarlyIterationValuesEqual)
                yield break;

            var addedItemsCount = 0;

            for (var rightValueIndexPairIndex = 0; rightValueIndexPairIndex < rightValueIndexPairs.Count; rightValueIndexPairIndex++) {
                var rightValueIndexPair = rightValueIndexPairs[rightValueIndexPairIndex];
                var foundLeftValueIndexPair = new ValueIndexPair<ValueIndexPair<T>>(default, -1);

                for (var leftValueIndexPairIndex = 0; leftValueIndexPairIndex < leftValueIndexPairs.Count; leftValueIndexPairIndex++) {
                    var leftValueIndexPair = leftValueIndexPairs[leftValueIndexPairIndex];

                    if (equalityComparer.Equals(leftValueIndexPair.Value, rightValueIndexPair.Value)) {
                        foundLeftValueIndexPair = new ValueIndexPair<ValueIndexPair<T>>(leftValueIndexPair, leftValueIndexPairIndex);
                        // We found a left value that is equal to the right value and want to cancel the search
                        break;
                    }
                }

                var change = default(CollectionChange<T>);

                if (foundLeftValueIndexPair.Index == -1) {
                    change = CollectionChange<T>.CreateNew(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index, rightValueIndexPair.Index);
                    addedItemsCount++;
                } else {
                    if (foundLeftValueIndexPair.Index == rightValueIndexPair.Index) {
                        change = new CollectionChange<T>(NotifyCollectionChangedAction.Replace, foundLeftValueIndexPair.Value.Value, foundLeftValueIndexPair.Value.Index, rightValueIndexPair.Value, -1, rightValueIndexPair.Index);
                    } else
                        change = new CollectionChange<T>(NotifyCollectionChangedAction.Move, foundLeftValueIndexPair.Value.Value, foundLeftValueIndexPair.Value.Index, rightValueIndexPair.Value, rightValueIndexPair.Index, rightValueIndexPair.Index);
                }

                if (change != null)
                    yield return change;
            }

            for (var leftValueIndexPairIndex = leftValueIndexPairs.Count - 1; leftValueIndexPairIndex >= rightValueIndexPairs.Count; leftValueIndexPairIndex--) {
                var leftValueIndexPair = leftValueIndexPairs[leftValueIndexPairIndex];
                var newLeftValueIndex = leftValueIndexPairIndex + addedItemsCount;
                var change = CollectionChange<T>.CreateOld(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, newLeftValueIndex);
                yield return change;
            }
        }

        public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionChanges(leftValues, rightValues, EqualityComparer<T>.Default);

        private class MutableValueIndexPair<T>
        {
            public int Index { get; set; }
            public T Value { get; set; }

            public MutableValueIndexPair(T value, int index)
            {
                Value = value;
                Index = index;
            }

            public override string ToString() => $"[{Value}, {Index}]";
        }
    }
}
