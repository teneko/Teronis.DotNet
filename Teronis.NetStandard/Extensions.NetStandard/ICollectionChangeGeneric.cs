using System.Linq;
using System.Collections.Generic;
using Teronis.Collections;

namespace Teronis.Extensions.NetStandard
{
    public static class CollectionChangeGenericExtensions
    {
        private static IList<T> getItems<T>(IEnumerable<T> collection, int collectionCount, int skipCount, int takeCount)
        {
            collection = collection
                .Skip(skipCount)
                .Take(takeCount);

            var oversteps = (skipCount + takeCount) - collectionCount;

            if (oversteps <= 0)
                return collection.ToList();
            else
                return collection.ContinueWith(Enumerable.Repeat<T>(default, oversteps)).ToList();
        }

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static IList<TChangedItem> GetOldItems<T, TChangedItem>(this CollectionChange<T> change, ICollection<TChangedItem> collection)
            => getItems(collection, collection.Count, change.OldIndex, change.OldItems.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static IList<TChangedItem> GetOldItems<T, TChangedItem>(this CollectionChange<T> change, IEnumerable<TChangedItem> collection, int collectionCount)
            => getItems(collection, collectionCount, change.OldIndex, change.OldItems.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static IList<T> GetNewItems<T>(this CollectionChange<T> change, ICollection<T> collection)
            => getItems(collection, collection.Count, change.NewIndex, change.OldItems.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static IList<T> GetNewItems<T>(this CollectionChange<T> change, IEnumerable<T> collection, int collectionCount)
            => getItems(collection, collectionCount, change.NewIndex, change.OldItems.Count);
    }
}
