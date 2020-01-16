using System.Collections.Generic;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<T> YieldReversedItems<T>(this LinkedList<T> list)
            => LinkedListTools.YieldReversedItems(list);
    }
}
