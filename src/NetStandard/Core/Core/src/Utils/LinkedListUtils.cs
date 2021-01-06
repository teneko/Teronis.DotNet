using System.Collections.Generic;

namespace Teronis.Utils
{
    public static class LinkedListUtils
    {
        public static IEnumerable<T> YieldItemsReversed<T>(LinkedList<T> list)
        {
            var node = list.Last;

            while (node != null) {
                yield return node.Value;
                node = node.Previous;
            }
        }
    }
}
