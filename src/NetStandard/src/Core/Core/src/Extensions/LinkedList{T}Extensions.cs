using System.Collections.Generic;

namespace Teronis.Extensions
{
    public static class LinkedListGenericExtensions
    {
        public static void Add<T>(this LinkedList<T> source, T item) => source.AddLast(item);
    }
}
