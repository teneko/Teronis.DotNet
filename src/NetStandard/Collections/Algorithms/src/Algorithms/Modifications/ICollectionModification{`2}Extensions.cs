// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.ObjectModel;
using Teronis.Extensions;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionModificationGenericExtensions
    {
        private static List<ItemType>? getItems<ItemType>(IEnumerable<ItemType> collection, int? nullableCollectionCount, int skipCount, int? nullableTakeCount)
        {
            int collectionCount, takeCount;

            if (nullableCollectionCount == null || nullableTakeCount == null) {
                return null;
            } else {
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
        /// Get the items from <paramref name="collection"/> beginning from <see cref="ICollectionModification{NewItemType, OldItemType}.OldIndex"/>.
        /// </summary>
        public static List<TargetItemType>? GetItemsBeginningFromOldIndex<NewItemType, OldItemType, TargetItemType>(
            this ICollectionModification<NewItemType, OldItemType> change,
            IReadOnlyCollection<TargetItemType> collection)
            => getItems(collection, collection?.Count, change.OldIndex, change.OldItems?.Count);

        /// <summary>
        /// Get the items from <paramref name="collection"/> beginning from <see cref="ICollectionModification{NewItemType, OldItemType}.OldIndex"/>.
        /// </summary>
        public static List<TargetItemType>? GetItemsBeginningFromOldIndex<NewItemType, OldItemType, TargetItemType>(
            this ICollectionModification<NewItemType, OldItemType> change,
            IEnumerable<TargetItemType> collection,
            int collectionCount)
            => getItems(collection, collectionCount, change.OldIndex, change.OldItems?.Count);

        /// <summary>
        /// Get the items from <paramref name="collection"/> beginning from <see cref="ICollectionModification{NewItemType, OldItemType}.NewIndex"/>.
        /// </summary>
        public static List<TargetItemType>? GetItemsBeginningFromNewIndex<NewItemType, OldItemType, TargetItemType>(
            this ICollectionModification<NewItemType, OldItemType> change,
            IReadOnlyCollection<TargetItemType> collection)
            => getItems(collection, collection?.Count, change.NewIndex, change.NewItems?.Count);

        /// <summary>
        /// Get the items from <paramref name="collection"/> beginning from <see cref="ICollectionModification{NewItemType, OldItemType}.NewIndex"/>.
        /// </summary>
        public static List<TargetItemType>? GetItemsBeginningFromNewIndex<NewItemType, OldItemType, TargetItemType>(
            this ICollectionModification<NewItemType, OldItemType> change,
            IEnumerable<TargetItemType> collection,
            int collectionCount)
            => getItems(collection, collectionCount, change.NewIndex, change.NewItems?.Count);

        /// <summary>
        /// Copies <paramref name="modification"/> but replaces <see cref="ICollectionModification{NewItemType, OldItemType}.OldItems"/>
        /// and <see cref="ICollectionModification{NewItemType, OldItemType}.NewItems"/>.
        /// </summary>
        /// <typeparam name="SourceOldItemType"></typeparam>
        /// <typeparam name="SourceNewItemType"></typeparam>
        /// <typeparam name="TargetOldItemType"></typeparam>
        /// <typeparam name="TargetNewItemType"></typeparam>
        /// <param name="modification"></param>
        /// <param name="otherOldItems"></param>
        /// <param name="otherNewItems"></param>
        /// <returns>New instance of <see cref="CollectionModification{NewItemType, OldItemType}"/>.</returns>
        public static CollectionModification<TargetNewItemType, TargetOldItemType> CopyWithOtherItems<SourceOldItemType, SourceNewItemType, TargetOldItemType, TargetNewItemType>(
            this ICollectionModification<SourceOldItemType, SourceNewItemType> modification,
            IReadOnlyList<TargetOldItemType>? otherOldItems,
            IReadOnlyList<TargetNewItemType>? otherNewItems)
        {
            modification = modification ?? throw new ArgumentNullException(nameof(modification));
            return new CollectionModification<TargetNewItemType, TargetOldItemType>(modification.Action, otherOldItems, modification.OldIndex, otherNewItems, modification.NewIndex);
        }

        public static CollectionModification<NewItemType, OldItemType> CopyWithOtherValues<NewItemType, OldItemType>(
            this ICollectionModification<NewItemType, OldItemType> modification,
            YetNullable<IReadOnlyList<OldItemType>>? oldItems = null,
            int? oldIndex = null,
            YetNullable<IReadOnlyList<NewItemType>>? newItems = null,
            int? newIndex = null)
        {
            modification = modification ?? throw new ArgumentNullException(nameof(modification));
            var _oldItems = oldItems.HasValue ? oldItems.Value.Value : modification.OldItems;
            var _oldIndex = oldIndex ?? modification.OldIndex;
            var _newItems = newItems.HasValue ? newItems.Value.Value : modification.NewItems;
            var _newIndex = newIndex ?? modification.NewIndex;
            return new CollectionModification<NewItemType, OldItemType>(modification.Action, _oldItems, _oldIndex, _newItems, _newIndex);
        }

        public static NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs<ItemType>(this ICollectionModification<ItemType, ItemType> modification)
        {
            var oldItemReadOnlyCollection = modification.OldItems is null ? null : new ReadOnlyList<ItemType>(modification.OldItems);
            var newItemReadOnlyCollection = modification.NewItems is null ? null : new ReadOnlyList<ItemType>(modification.NewItems);

            return NotifyCollectionChangedEventArgsUtils.CreateNotifyCollectionChangedEventArgs(
                modification.Action,
                oldItemReadOnlyCollection,
                modification.OldIndex,
                newItemReadOnlyCollection,
                modification.NewIndex);
        }
    }
}
