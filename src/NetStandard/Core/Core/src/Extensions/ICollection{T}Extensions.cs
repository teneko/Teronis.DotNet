using System;
using System.Collections.Generic;

namespace Teronis.Extensions
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

        public static ListType Include<ListType, ItemType>(this ListType original, IEnumerable<ItemType> source)
            where ListType : ICollection<ItemType>
        {
            original = original ?? throw new ArgumentNullException(nameof(original));
            source = source ?? throw new ArgumentNullException(nameof(source));

            foreach (var item in source) {
                original.Add(item);
            }

            return original;
        }
    }
}
