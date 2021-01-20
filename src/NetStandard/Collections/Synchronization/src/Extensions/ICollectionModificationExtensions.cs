using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Algorithms;

namespace Teronis.Collections.Synchronization.Extensions
{
    public static class ICollectionModificationExtensions
    {
        public static IEnumerable<(OldItemType OldItem, NewItemType NewItem)> YieldTuplesForOldItemNewItemReplace<OldItemType, NewItemType>(this ICollectionModification<OldItemType, NewItemType> change)
        {
            if (change.Action != NotifyCollectionChangedAction.Replace) {
                throw new ArgumentException("Replace action was expected.");
            }

            var oldItems = change.OldItems ??
                throw new ArgumentException("The old items was null.");

            var newItems = change.NewItems ??
                throw new ArgumentException("The new items was null.");

            var oldItemsEnumerator = oldItems.GetEnumerator();
            var newItemsEnumerator = newItems.GetEnumerator();

            bool hasNextOldItem, hasNextNewItem;

            while ((hasNextOldItem = oldItemsEnumerator.MoveNext()) & (hasNextNewItem = newItemsEnumerator.MoveNext())) {
                var oldItem = oldItemsEnumerator.Current;
                var newItem = newItemsEnumerator.Current;
                yield return (OldItem: oldItem, NewItem: newItem);
            }

            if (!(!hasNextOldItem && !hasNextNewItem)) {
                throw new ArgumentException("The replace modification expects old items and new items to be even.");
            }
        }

        public static IEnumerable<(int OldIndex, NewItemType NewItem)> YieldTuplesForOldIndexNewItemReplace<OldItemType, NewItemType>(
            this ICollectionModification<OldItemType, NewItemType> modification)
        {
            var newItems = modification.NewItems;

            if (newItems is null) {
                throw CollectionModificationThrowHelper.NewItemsWereNullException();
            }

            int oldItemIndex = modification.OldIndex;
            var itemEnumerator = newItems.GetEnumerator();

            while (itemEnumerator.MoveNext()) {
                yield return (OldIndex: oldItemIndex, NewItem: itemEnumerator.Current);
                oldItemIndex++;
            }
        }
    }
}
