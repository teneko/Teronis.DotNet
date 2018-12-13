using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.NetStandard.Collections.Generic;

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

            foreach (T value in sequence) {
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
        public static IEnumerable<ItemComparison<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues, IEqualityComparer<T> equalityComparer)
        {
            var ttbRemovedEntities = new List<ItemComparison<T>>();

            foreach (var leftValue in leftValues) {
                var otherEntity = rightValues.SingleOrDefault(x => equalityComparer.Equals(x, leftValue));

                if (Tools.Tools.ReturnBoolValue(rightValues.FirstOrDefault(x => equalityComparer.Equals(x, leftValue)), out var updateToValue, (_updateToValue) => !equalityComparer.Equals(_updateToValue, default)))
                    yield return new ItemComparison<T>(leftValue, ECompareOperator.Update, updateToValue);
                else
                    ttbRemovedEntities.Add(new ItemComparison<T>(leftValue, ECompareOperator.Remove));
            }

            foreach (var ttbRemovedEntity in ttbRemovedEntities)
                yield return ttbRemovedEntity;

            foreach (var rightValue in rightValues)
                if (!leftValues.Any(x => equalityComparer.Equals(x, rightValue)))
                    yield return new ItemComparison<T>(ECompareOperator.Add, rightValue);
        }

        public static IEnumerable<ItemComparison<T>> GetCollectionDifferences<T>(this IEnumerable<T> leftValues, IEnumerable<T> rightValues)
            => GetCollectionDifferences(leftValues, rightValues, EqualityComparer<T>.Default);
    }
}
