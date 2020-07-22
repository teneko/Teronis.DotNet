using System.Collections.Generic;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<T> YieldReversedItems<T>(this LinkedList<T> list) =>
            LinkedListUtils.YieldReversedItems(list);
    }
}
