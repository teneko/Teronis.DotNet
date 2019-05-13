using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections;
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
        /// Add / Replace / Remove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftValues"></param>
        /// <param name="rightValues"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static IEnumerable<CollectionChange<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        {
            var leftValuesEnumerator = leftValues.GetEnumerator();
            var rightValuesEnumerator = rightValues.GetEnumerator();
            var hasLeftValue = true;
            var hasRightValue = true;
            var leftValueIndex = 0;
            var rightValueIndex = 0;
            var leftValueIndexPairs = new List<ValueIndexPair<T>>();
            var rightValueIndexPairs = new List<ValueIndexPair<T>>();
            var areEarlyIterationValuesEqual = true;

            do {
                if (hasLeftValue)
                    hasLeftValue = leftValuesEnumerator.MoveNext();

                if (hasRightValue)
                    hasRightValue = rightValuesEnumerator.MoveNext();

                // Cancel if not a left or right value is available
                if (!hasLeftValue || !hasRightValue)
                    break;

                var leftValue = leftValuesEnumerator.Current;
                var rightValue = rightValuesEnumerator.Current;

                if (hasLeftValue && hasRightValue) {
                    if (equalityComparer.Equals(leftValue, rightValue))
                        // Changes on the same index can always be returned immediately
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, rightValueIndex);
                    else {
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));
                        areEarlyIterationValuesEqual = false;
                    }

                    leftValueIndex++;
                    rightValueIndex++;
                } else if (hasLeftValue) {
                    if (areEarlyIterationValuesEqual)
                        yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValue, leftValueIndex);
                    else
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));

                    leftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValue, rightValueIndex);
                    else
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));

                    rightValueIndex++;
                }
            } while (true);

            // Exit only if both lists were synced from early on
            if (areEarlyIterationValuesEqual)
                yield break;

            if (rightValueIndexPairs.Count != 0) {
                foreach (var rightValueIndexPair in rightValueIndexPairs)
                    yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);
            } else if (leftValueIndexPairs.Count != 0) {
                foreach (var leftValueIndexPair in leftValueIndexPairs)
                    yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.Index);
            } else {
                var ttbRemovedValues = new List<CollectionChange<T>>();
                var ttbMovedValues = new List<CollectionChange<T>>();
                //var ttbAddedValues = new List<ValueIndexPair<T>>();

                for (leftValueIndex = leftValueIndexPairs.Count - 1; leftValueIndex >= 0; leftValueIndex--) {
                    var leftValueIndexPair = leftValueIndexPairs[leftValueIndex];
                    ValueIndexPair<ValueIndexPair<T>>? foundRightValueIndexPair = null;

                    for (rightValueIndex = rightValueIndexPairs.Count - 1; rightValueIndex >= 0; rightValueIndex--) {
                        var rightValueIndexPair = rightValueIndexPairs[rightValueIndex];

                        if (equalityComparer.Equals(leftValueIndexPair))
                            foundRightValueIndexPair = new ValueIndexPair<ValueIndexPair<T>>(rightValueIndexPair, rightValueIndex);
                    }

                    if (foundRightValueIndexPair == null) {
                        ttbRemovedValues.Add(CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.Index));
                    } else {
                        ttbMovedValues.Add(new CollectionChange<T>(NotifyCollectionChangedAction.Move, leftValueIndexPair.Value, leftValueIndexPair.Index, foundRightValueIndexPair.Value.Value.Value, foundRightValueIndexPair.Value.Value.Index));
                        rightValueIndexPairs.RemoveAt(foundRightValueIndexPair.Value.Index);
                    }

                    leftValueIndexPairs.RemoveAt(leftValueIndex);
                }

                foreach (var rightValueIndexPair in rightValueIndexPairs)
                    yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);
            }
        }

        public static IEnumerable<CollectionChange<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionDifferences(leftValues, rightValues, EqualityComparer<T>.Default);
    }
}
