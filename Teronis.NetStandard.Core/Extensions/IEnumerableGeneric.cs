using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.NetStandard.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.NetStandard.Extensions
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
        /// update -> remove -> add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftValues"></param>
        /// <param name="rightValues"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static IEnumerable<CollectionChange<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        {
            //var tttbRemovedEntities = new List<ItemComparison>

            var leftValuesEnumerator = leftValues.GetEnumerator();
            var rightValuesEnumerator = rightValues.GetEnumerator();
            bool hasLeftValue, hasRightValue;
            var leftValueIndex = 0;
            var rightValueIndex = 0;
            var leftValueIndexPairs = new List<ValueIndexPair<T>>();
            var rightValueIndexPairs = new List<ValueIndexPair<T>>();
            var isFirstIterationSynced = true;

            while ((hasLeftValue = leftValuesEnumerator.MoveNext()) | (hasRightValue = rightValuesEnumerator.MoveNext())) {
                var leftValue = leftValuesEnumerator.Current;
                var rightValue = rightValuesEnumerator.Current;

                if (hasLeftValue && hasRightValue) {
                    if (equalityComparer.Equals(leftValue, rightValue))
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, rightValueIndex);
                    else {
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));
                        isFirstIterationSynced = false;
                    }

                    leftValueIndex++;
                    rightValueIndex++;
                } else if (hasLeftValue) {
                    if (isFirstIterationSynced)
                        leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));
                    else
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Remove, leftValue, leftValueIndex);
                } else {
                    if (isFirstIterationSynced)
                        rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));
                    else
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Add, rightValue, rightValueIndex);

                    rightValueIndex++;
                }
            }

            if (isFirstIterationSynced)
                yield break;

            if (leftValueIndexPairs.Count == 0) {
                foreach (var rightValueIndexPair in rightValueIndexPairs)
                    yield return new CollectionChange<T>(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);
            } else if (rightValueIndexPairs.Count == 0) {
                foreach (var leftValueIndexPair in leftValueIndexPairs)
                    yield return new CollectionChange<T>(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.Index);
            } else {
                var leftValueIndexPairsCount = leftValueIndexPairs.Count;
                var rightValueIndexPairsCount = rightValueIndexPairs.Count;
                var ttbRemovedValues = new List<CollectionChange<T>>();
                var ttbMovedValues = new List<CollectionChange<T>>();
                var ttbAddedValues = new List<ValueIndexPair<T>>();

                for (leftValueIndex = leftValueIndexPairs.Count - 1; leftValueIndex >= 0; leftValueIndex--) {
                    var leftValueIndexPair = leftValueIndexPairs[leftValueIndex];
                    ValueIndexPair<ValueIndexPair<T>>? foundRightValueIndexPair = null;

                    for (rightValueIndex = rightValueIndexPairs.Count - 1; rightValueIndex >= 0; rightValueIndex--) {
                        var rightValueIndexPair = rightValueIndexPairs[rightValueIndex];

                        if (equalityComparer.Equals(leftValueIndexPair))
                            foundRightValueIndexPair = new ValueIndexPair<ValueIndexPair<T>>(rightValueIndexPair, rightValueIndex);
                    }

                    if (foundRightValueIndexPair == null) {
                        ttbRemovedValues.Add(new CollectionChange<T>(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.Index));
                    } else {
                        ttbMovedValues.Add(new CollectionChange<T>(NotifyCollectionChangedAction.Move, leftValueIndexPair.Value, leftValueIndexPair.Index, foundRightValueIndexPair.Value.Value.Value, foundRightValueIndexPair.Value.Value.Index));
                        rightValueIndexPairs.RemoveAt(foundRightValueIndexPair.Value.Index);
                    }

                    leftValueIndexPairs.RemoveAt(leftValueIndex);
                }



                foreach (var rightValueIndexPair in rightValueIndexPairs)
                    yield return new CollectionChange<T>(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);
            }
        }

        //if (!leftValuesHasItem && rightValuesHasItem)

        //var ttbRemovedEntities = new List<ItemComparison<T>>();

        //foreach (var leftValue in leftValues) {
        //    var otherEntity = rightValues.SingleOrDefault(x => equalityComparer.Equals(x, leftValue));

        //    if (Tools.Tools.ReturnBoolValue(rightValues.FirstOrDefault(x => equalityComparer.Equals(x, leftValue)), out var updateToValue, (_updateToValue) => !equalityComparer.Equals(_updateToValue, default)))
        //        yield return new ItemComparison<T>(leftValue, ECompareOperator.Update, updateToValue);
        //    else
        //        ttbRemovedEntities.Add(new ItemComparison<T>(leftValue, ECompareOperator.Remove));
        //}

        //foreach (var ttbRemovedEntity in ttbRemovedEntities)
        //    yield return ttbRemovedEntity;

        //foreach (var rightValue in rightValues)
        //    if (!leftValues.Any(x => equalityComparer.Equals(x, rightValue)))
        //        yield return new ItemComparison<T>(ECompareOperator.Add, rightValue);

        public static IEnumerable<CollectionChange<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionDifferences(leftValues, rightValues, EqualityComparer<T>.Default);
    }
}
