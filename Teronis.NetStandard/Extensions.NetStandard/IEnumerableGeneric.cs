using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections;
using Teronis.Collections.Generic;
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

        ///// <summary>
        ///// Add / Replace / Remove
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="leftValues"></param>
        ///// <param name="rightValues"></param>
        ///// <param name="equalityComparer"></param>
        ///// <returns></returns>
        //public static IEnumerable<CollectionChange<T>> GetCollectionChanges<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        //{
        //    rightValues = rightValues.ToList().ReturnedShuffle();

        //    var leftValuesEnumerator = leftValues.GetEnumerator();
        //    var rightValuesEnumerator = rightValues.GetEnumerator();
        //    var hasLeftValue = true;
        //    var hasRightValue = true;
        //    var leftValueIndex = 0;
        //    var rightValueIndex = 0;
        //    var leftValueIndexPairs = new List<ValueIndexPair<T>>();
        //    var rightValueIndexPairs = new List<ValueIndexPair<T>>();
        //    var areEarlyIterationValuesEqual = true;

        //    do {
        //        T leftValue;
        //        T rightValue;

        //        if (hasLeftValue) {
        //            hasLeftValue = leftValuesEnumerator.MoveNext();
        //        }

        //        if (hasRightValue)
        //            hasRightValue = rightValuesEnumerator.MoveNext();

        //        // Cancel if not a left or right value is available
        //        //if (!hasLeftValue || !hasRightValue)
        //        if (!(hasLeftValue | hasRightValue))
        //            break;

        //        if (hasLeftValue)
        //            leftValue = leftValuesEnumerator.Current;
        //        else
        //            leftValue = default; // Only for compiler

        //        if (hasRightValue)
        //            rightValue = rightValuesEnumerator.Current;
        //        else
        //            rightValue = default; // Only for compiler

        //        if (hasLeftValue && hasRightValue) {
        //            if (equalityComparer.Equals(leftValue, rightValue))
        //                // Changes on the same index can always be returned immediately
        //                yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, rightValueIndex);
        //            else {
        //                leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));
        //                rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));
        //                areEarlyIterationValuesEqual = false;
        //            }

        //            leftValueIndex++;
        //            rightValueIndex++;
        //        } else if (hasLeftValue) {
        //            if (areEarlyIterationValuesEqual)
        //                yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValue, leftValueIndex);
        //            else
        //                leftValueIndexPairs.Add(new ValueIndexPair<T>(leftValue, leftValueIndex));

        //            leftValueIndex++;
        //        } else {
        //            if (areEarlyIterationValuesEqual)
        //                yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValue, rightValueIndex);
        //            else
        //                rightValueIndexPairs.Add(new ValueIndexPair<T>(rightValue, rightValueIndex));

        //            rightValueIndex++;
        //        }
        //    } while (true);

        //    // Exit only if both lists were synced from early on
        //    if (areEarlyIterationValuesEqual)
        //        yield break;

        //    // If only right values are there, then we just have to return those
        //    if (leftValueIndexPairs.Count == 0 && rightValueIndexPairs.Count != 0) {
        //        foreach (var rightValueIndexPair in rightValueIndexPairs)
        //            yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);
        //    }
        //    // But if only left values are there, then we just have to return those
        //    else if (rightValueIndexPairs.Count == 0 && leftValueIndexPairs.Count != 0) {
        //        foreach (var leftValueIndexPair in leftValueIndexPairs)
        //            yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValueIndexPair.Value, leftValueIndexPair.Index);
        //    } else {
        //        var removableValueIndexPairs = new List<ValueIndexPair<T>>();
        //        //// Moving values implies the actions remove, add and replace
        //        //var movableValues = new List<CollectionChange<T>>();
        //        var addableValues = new List<ValueIndexPair<T>>();
        //        // Replace is after all else actions
        //        var replacableValues = new List<CollectionChange<T>>();

        //        /* We want to check, if each left value does exist on right side. 
        //         * If so, we move it, otherwise we remove it.
        //         */
        //        for (leftValueIndex = leftValueIndexPairs.Count - 1; leftValueIndex >= 0; leftValueIndex--) {
        //            var leftValueIndexPair = leftValueIndexPairs[leftValueIndex];
        //            ValueIndexPair<ValueIndexPair<T>>? foundRightValueIndexPair = null;

        //            for (rightValueIndex = rightValueIndexPairs.Count - 1; rightValueIndex >= 0; rightValueIndex--) {
        //                var rightValueIndexPair = rightValueIndexPairs[rightValueIndex];

        //                if (equalityComparer.Equals(leftValueIndexPair.Value, rightValueIndexPair.Value)) {
        //                    foundRightValueIndexPair = new ValueIndexPair<ValueIndexPair<T>>(rightValueIndexPair, rightValueIndex);
        //                    // We found a right value that is equal to the current left value and want to cancel the search
        //                    break;
        //                }
        //            }

        //            if (foundRightValueIndexPair == null)
        //                removableValueIndexPairs.Add(leftValueIndexPair);
        //            // We do NOT need to check for equal value value indexes, they are already handled (see top)
        //            else {
        //                removableValueIndexPairs.Add(leftValueIndexPair);
        //                addableValues.Add(new ValueIndexPair<T>(leftValueIndexPair.Value, foundRightValueIndexPair.Value.Value.Index));
        //                replacableValues.Add(new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValueIndexPair.Value, foundRightValueIndexPair.Value.Value.Index, foundRightValueIndexPair.Value.Value.Value, foundRightValueIndexPair.Value.Value.Index));
        //                rightValueIndexPairs.RemoveAt(foundRightValueIndexPair.Value.Index);
        //            }

        //            leftValueIndexPairs.RemoveAt(leftValueIndex);
        //        }

        //        // We need to sort the removing values by index descending
        //        removableValueIndexPairs.Sort((x, y) => y.Index.CompareTo(x.Index));
        //        // We want add these right values that are left
        //        addableValues.AddRange(rightValueIndexPairs);
        //        // Now we need to sort the adding values by index ascending
        //        addableValues.Sort((x, y) => x.Index.CompareTo(y.Index));

        //        // Then we notify about removable values
        //        foreach (var removableValueIndexPair in removableValueIndexPairs)
        //            yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, removableValueIndexPair.Value, removableValueIndexPair.Index);

        //        // .. about addable values
        //        foreach (var rightValueIndexPair in addableValues)
        //            yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValueIndexPair.Value, rightValueIndexPair.Index);

        //        // .. and replacable values
        //        foreach (var replacableValue in replacableValues)
        //            yield return replacableValue;
        //    }
        //}

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
            rightValues = rightValues.ToList().ReturnedShuffle();

            var leftValuesEnumerator = leftValues.GetEnumerator();
            var rightValuesEnumerator = rightValues.GetEnumerator();
            var hasLeftValue = true;
            var hasRightValue = true;
            var leftValueIndex = 0;
            var rightValueIndex = 0;
            var leftMutableValueIndexPairs = new List<MutableValueIndexPair<T>>();
            var rightMutableValueIndexPairs = new List<MutableValueIndexPair<T>>();
            var areEarlyIterationValuesEqual = true;
            var indexShiftedNotifier = new IndexShiftedNotifier();

            do {
                T leftValue;
                T rightValue;

                if (hasLeftValue) {
                    hasLeftValue = leftValuesEnumerator.MoveNext();
                }

                if (hasRightValue)
                    hasRightValue = rightValuesEnumerator.MoveNext();

                // Cancel if not a left or right value is available
                //if (!hasLeftValue || !hasRightValue)
                if (!(hasLeftValue | hasRightValue))
                    break;

                if (hasLeftValue)
                    leftValue = leftValuesEnumerator.Current;
                else
                    leftValue = default; // Only for compiler

                if (hasRightValue)
                    rightValue = rightValuesEnumerator.Current;
                else
                    rightValue = default; // Only for compiler

                if (hasLeftValue && hasRightValue) {
                    if (equalityComparer.Equals(leftValue, rightValue))
                        // Changes on the same index can always be returned immediately
                        yield return new CollectionChange<T>(NotifyCollectionChangedAction.Replace, leftValue, leftValueIndex, rightValue, rightValueIndex);
                    else {
                        leftMutableValueIndexPairs.Add(new MutableValueIndexPair<T>(leftValue, leftValueIndex));
                        rightMutableValueIndexPairs.Add(new MutableValueIndexPair<T>(rightValue, rightValueIndex));
                        areEarlyIterationValuesEqual = false;
                    }

                    leftValueIndex++;
                    rightValueIndex++;
                } else if (hasLeftValue) {
                    if (areEarlyIterationValuesEqual)
                        yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftValue, leftValueIndex);
                    else
                        leftMutableValueIndexPairs.Add(new MutableValueIndexPair<T>(leftValue, leftValueIndex));

                    leftValueIndex++;
                } else {
                    if (areEarlyIterationValuesEqual)
                        yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightValue, rightValueIndex);
                    else
                        rightMutableValueIndexPairs.Add(new MutableValueIndexPair<T>(rightValue, rightValueIndex));

                    rightValueIndex++;
                }
            } while (true);

            // Exit only if both lists were synced from early on
            if (areEarlyIterationValuesEqual)
                yield break;

            //// If only right values are there, then we just have to return those
            //if (leftMutableValueIndexPairs.Count == 0 && rightMutableValueIndexPairs.Count != 0) {
            //    foreach (var rightMutableValueIndexPair in rightMutableValueIndexPairs)
            //        yield return CollectionChange<T>.CreateRight(NotifyCollectionChangedAction.Add, rightMutableValueIndexPair.Value, rightMutableValueIndexPair.Index);
            //}
            //// But if only left values are there, then we just have to return those
            //else if (rightMutableValueIndexPairs.Count == 0 && leftMutableValueIndexPairs.Count != 0) {
            //    foreach (var leftMutableValueIndexPair in leftMutableValueIndexPairs)
            //        yield return CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftMutableValueIndexPair.Value, leftMutableValueIndexPair.Index);
            //} else {
            //var removableMutableValueIndexPairs = new List<MutableValueIndexPair<T>>();
            var removableChanges = new List<CollectionChange<T>>();
            //// Moving values implies the actions remove, add and replace
            //var movableValues = new List<CollectionChange<T>>();
            //var addableValues = new List<MutableValueIndexPair<T>>();
            var addableChanges = rightMutableValueIndexPairs.Select(x => CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Add, x.Value, x.Index, indexShiftedNotifier));
            // Replace is after all else actions
            var replacableValues = new List<CollectionChange<T>>();

            var movableValues = new List<CollectionChange<T>>();

            /* We want to check, if each left value does exist on right side. 
             * If so, we move it, otherwise we remove it.
             */
            for (leftValueIndex = leftMutableValueIndexPairs.Count - 1; leftValueIndex >= 0; leftValueIndex--) {
                var leftMutableValueIndexPair = leftMutableValueIndexPairs[leftValueIndex];
                MutableValueIndexPair<MutableValueIndexPair<T>> foundRightMutableValueIndexPair = null;

                for (rightValueIndex = rightMutableValueIndexPairs.Count - 1; rightValueIndex >= 0; rightValueIndex--) {
                    var rightMutableValueIndexPair = rightMutableValueIndexPairs[rightValueIndex];

                    if (equalityComparer.Equals(leftMutableValueIndexPair.Value, rightMutableValueIndexPair.Value)) {
                        foundRightMutableValueIndexPair = new MutableValueIndexPair<MutableValueIndexPair<T>>(rightMutableValueIndexPair, rightValueIndex);
                        // We found a right value that is equal to the current left value and want to cancel the search
                        break;
                    }
                }

                if (foundRightMutableValueIndexPair == null) {
                    var change = CollectionChange<T>.CreateLeft(NotifyCollectionChangedAction.Remove, leftMutableValueIndexPair.Value, leftMutableValueIndexPair.Index, indexShiftedNotifier);
                    removableChanges.Add(change);
                }
                // We do NOT need to check for equal value value indexes, they are already handled (see top)
                else {
                    var change = new CollectionChange<T>(NotifyCollectionChangedAction.Move,
                        leftMutableValueIndexPair.Value,
                        leftMutableValueIndexPair.Index,
                        foundRightMutableValueIndexPair.Value.Value,
                        foundRightMutableValueIndexPair.Value.Index,
                        indexShiftedNotifier);

                    movableValues.Add(change);
                    rightMutableValueIndexPairs.RemoveAt(foundRightMutableValueIndexPair.Index);
                }

                leftMutableValueIndexPairs.RemoveAt(leftValueIndex);
            }

            //// We need to sort the removing values by index descending
            //removableMutableValueIndexPairs.Sort((x, y) => y.Index.CompareTo(x.Index));
            //// Now we need to sort the adding values by index ascending
            //addableValues.Sort((x, y) => x.Index.CompareTo(y.Index));

            foreach (var change in movableValues) {
                var objectifiedChange = (ICollectionChange<object>)change;
                indexShiftedNotifier.ShiftIndex(objectifiedChange);
            }

            // Then we notify about removable changes
            foreach (var change in removableChanges) {
                yield return change;
                var objectifiedChange = (ICollectionChange<object>)change;
                indexShiftedNotifier.ShiftIndex(objectifiedChange);
            }

            // .. about addable changes
            foreach (var change in addableChanges) {
                yield return change;
                var objectifiedChange = (ICollectionChange<object>)change;
                indexShiftedNotifier.ShiftIndex(objectifiedChange);
            }

            // .. and replacable values
            //foreach (var replacableValue in replacableValues)
            //    yield return replacableValue;
            //}
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
