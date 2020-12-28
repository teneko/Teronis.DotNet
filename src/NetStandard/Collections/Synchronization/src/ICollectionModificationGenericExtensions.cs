using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public static class ICollectionModificationGenericExtensions
    {
        public static IEnumerable<(OldItemType OldItem, NewItemType NewItem)> GetReplaceItemsIterator<OldItemType, NewItemType>(this ICollectionModification<OldItemType, NewItemType> change)
        {
            if (change.Action == NotifyCollectionChangedAction.Replace) {
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
        }
    }
}
