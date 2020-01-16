using System.Collections.Generic;
using Teronis.Collections;
using Teronis.Collections.Specialized;

namespace Teronis.Extensions
{
    public static class IIndexedEnumerableExtensions
    {
        public static IIndexedEnumerable<T> AsCovariant<T>(this IList<T> list) => new CovariantList<T>(list);
    }
}
