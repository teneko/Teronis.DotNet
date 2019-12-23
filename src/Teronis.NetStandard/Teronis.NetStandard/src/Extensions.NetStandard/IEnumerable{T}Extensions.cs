using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.Collections.Generic;
using Teronis.Extensions.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static partial class IEnumerableGenericExtensions
    {
        public static IEnumerable<T> ExcludeNulls<T>(this IEnumerable<T> collection) where T : class => collection.Where(x => x != null);

        public static R FirstNonDefaultOrDefault<T, R>(this IEnumerable<T> collection, Func<T, R> getObj)
        {
            var defaultObj = default(R);

            foreach (var item in collection)
            {
                var obj = getObj(item);

                if (!Equals(obj, defaultObj))
                    return obj;
            }

            return defaultObj;
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

            foreach (var value in sequence)
            {
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
    }
}
