using System.Collections.Generic;

namespace Teronis.Extensions.NetStandard
{
    public static class ICollectionGenericExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> anyCollection) => anyCollection == default || anyCollection.Count == 0;

        public static bool TryAdd<T>(this ICollection<T> list, T item)
        {
            if (!list.Contains(item)) {
                list.Add(item);
                return true;
            }
            //
            return false;
        }

        public static T AddAndReturn<T>(this ICollection<T> source, T item)
        {
            source.Add(item);
            return item;
        }
    }
}
