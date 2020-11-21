using System.Collections.Generic;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class LinkedListGenericExtensions
    {
        public static void Add<T>(this LinkedList<T> source, T item) =>
            source.AddLast(item);

        public static IEnumerable<T> YieldReversedItems<T>(this LinkedList<T> list) =>
            LinkedListUtils.YieldReversedItems(list);
    }
}
