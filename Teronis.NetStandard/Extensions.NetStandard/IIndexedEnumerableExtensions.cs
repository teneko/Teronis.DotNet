using System;
using System.Collections.Generic;
using System.Text;
using Teronis.Collections;
using Teronis.Collections.Specialized;
using Teronis.Data;

namespace Teronis.Extensions.NetStandard
{
    public static class IIndexedEnumerableExtensions
    {
        public static IIndexedEnumerable<T> AsCovariant<T>(this IList<T> list) => new CovariantList<T>(list);
    }
}
