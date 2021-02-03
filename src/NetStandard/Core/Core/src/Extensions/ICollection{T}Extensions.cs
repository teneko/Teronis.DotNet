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

        public static T AddAndReturnItem<T>(this ICollection<T> source, T item)
        {
            source.Add(item);
            return item;
        }

        public static CollectionType AddItemAndReturnList<CollectionType, T>(this CollectionType source, T item)
            where CollectionType : ICollection<T>
        {
            source.Add(item);
            return source;
        }

        public static ListType AddItemRangeAndReturnList<ListType, ItemType>(this ListType target, IEnumerable<ItemType> source)
            where ListType : ICollection<ItemType>
        {
            target = target ?? throw new ArgumentNullException(nameof(target));
            source = source ?? throw new ArgumentNullException(nameof(source));

            foreach (var item in source) {
                target.Add(item);
            }

            return target;
        }
    }
}
