using System;
using System.Collections.Generic;

namespace Teronis.Extensions.NetStandard
{
    public static class ListGenericExtensions
    {
        public static List<T> SortAndReturn<T>(this List<T> list, int index, int count, IComparer<T> comparer)
        {
            list.Sort(index, count, comparer);
            return list;
        }

        public static List<T> SortAndReturn<T>(this List<T> list, Comparison<T> comparison)
        {
            list.Sort(comparison);
            return list;
        }

        public static List<T> SortAndReturn<T>(this List<T> list, IComparer<T> comparer) => SortAndReturn(list, 0, list.Count, comparer);

        public static List<T> SortAndReturn<T>(this List<T> list) => SortAndReturn(list, Comparer<T>.Default);

        public static List<T> AddRangeAndReturn<T>(this List<T> source, IEnumerable<T> ttbList)
        {
            source.AddRange(ttbList);
            return source;
        }
    }
}
