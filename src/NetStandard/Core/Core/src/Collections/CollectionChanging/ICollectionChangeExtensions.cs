using System.Linq;
using System.Collections.Generic;
using Teronis.Extensions;

namespace Teronis.Collections.CollectionChanging
{
    public static class CollectionChangeGenericExtensions
    {
        private static List<ItemType>? getItems<ItemType>(IEnumerable<ItemType> collection, int? nullableCollectionCount, int skipCount, int? nullableTakeCount)
        {
            int collectionCount, takeCount;

            if (nullableCollectionCount == null || nullableTakeCount == null)
                return null;
            else {
                collectionCount = nullableCollectionCount.Value;
                takeCount = nullableTakeCount.Value;
            }

            collection = collection
                .Skip(skipCount)
                .Take(takeCount);

            var oversteps = skipCount + takeCount - collectionCount;

            if (oversteps <= 0) {
                return collection.ToList();
            } else {
                return collection.ContinueWith(Enumerable.Repeat<ItemType>(default!, oversteps)).ToList();
            }
        }

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static List<TargetItemType>? GetOldItems<OldItemType, NewItemType, TargetItemType>(this ICollectionChange<OldItemType, NewItemType> change, ICollection<TargetItemType> collection)
            => getItems(collection, collection?.Count, change.OldIndex, change.OldItems?.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static List<TargetItemType>? GetOldItems<OldItemType, NewItemType, TargetItemType>(this ICollectionChange<OldItemType, NewItemType> change, IEnumerable<TargetItemType> collection, int collectionCount)
            => getItems(collection, collectionCount, change.OldIndex, change.OldItems?.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static List<TargetItemType>? GetNewItems<OldItemType, NewItemType, TargetItemType>(this ICollectionChange<OldItemType, NewItemType> change, ICollection<TargetItemType> collection)
            => getItems(collection, collection?.Count, change.NewIndex, change.NewItems?.Count);

        /// <summary>
        /// Get the items before the change got applied on <paramref name="collection"/>.
        /// </summary>
        public static List<TargetItemType>? GetNewItems<OldItemType, NewItemType, TargetItemType>(this ICollectionChange<OldItemType, NewItemType> change, IEnumerable<TargetItemType> collection, int collectionCount)
            => getItems(collection, collectionCount, change.NewIndex, change.NewItems?.Count);

        public static CollectionChange<TargetOldItemType, TargetNewItemType> CreateOf<SourceOldItemType, SourceNewItemType, TargetOldItemType, TargetNewItemType>(this ICollectionChange<SourceOldItemType, SourceNewItemType> change, IReadOnlyList<TargetOldItemType> oldItems, IReadOnlyList<TargetNewItemType> newItems)
            => new CollectionChange<TargetOldItemType, TargetNewItemType>(change.Action, oldItems, change.OldIndex, newItems, change.NewIndex);
    }
}
