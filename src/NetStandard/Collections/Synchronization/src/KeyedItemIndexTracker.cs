using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public class KeyedItemIndexTracker<ItemType, KeyType>
    {
        private readonly ISynchronizableItemCollection<ItemType> itemCollection;
        private readonly Func<ItemType, KeyType> getItemKey;
        private readonly Dictionary<KeyType, int> itemIndexByKeyDictionary;

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey,
            IEqualityComparer<KeyType>? keyEqualityComparer)
        {
            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<KeyType>.Default;
            itemIndexByKeyDictionary = new Dictionary<KeyType, int>(keyEqualityComparer);
            this.itemCollection = itemCollection;
            this.getItemKey = getItemKey ?? throw new ArgumentNullException(nameof(getItemKey));
            itemCollection.CollectionModified += ModificationNotifier_CollectionModified;
            recalculateItemKeys();
        }

        private void recalculateItemKeys()
        {
            if (itemIndexByKeyDictionary.Count != 0) {
                itemIndexByKeyDictionary.Clear();
            }

            var itemCollectionCount = itemCollection.Count;

            for (var index = 0; index < itemCollectionCount; index++) {
                var item = itemCollection[index];
                var itemKey = getItemKey(item);
                itemIndexByKeyDictionary[itemKey] = index;
            }
        }

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey)
            : this(itemCollection, getItemKey, default(IEqualityComparer<KeyType>)) { }

        protected virtual KeyType GetItemKey(ItemType item) =>
            getItemKey(item);

        private void ModificationNotifier_CollectionModified(object sender, ICollectionModification<ItemType, ItemType> modification)
        {
            void replaceItemIndexes(IEnumerable<ItemType> items, int offset)
            {
                var itemsEnumerator = items.GetEnumerator();
                var index = 0;

                while (itemsEnumerator.MoveNext()) {
                    var itemKey = GetItemKey(itemsEnumerator.Current);
                    itemIndexByKeyDictionary[itemKey] = offset + index;
                    index++;
                }
            }

            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (modification.NewItems is null) {
                        throw CollectionModificationThrowHelper.NewItemsWereNullException();
                    }

                    replaceItemIndexes(modification.NewItems, modification.NewIndex);
                    break;
                case NotifyCollectionChangedAction.Remove: {
                        if (modification.OldItems is null) {
                            throw CollectionModificationThrowHelper.OldItemsWereNullException();
                        }

                        var oldItemsCount = modification.OldItems.Count;

                        for (var index = 0; index < oldItemsCount; index++) {
                            var itemKey = GetItemKey(modification.OldItems[index]);
                            itemIndexByKeyDictionary.Remove(itemKey);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Replace has not affect on calculated keys.
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (modification.NewItems is null) {
                        throw CollectionModificationThrowHelper.NewItemsWereNullException();
                    }

                    var amountOfAffectedItems = Math.Abs(modification.OldIndex - modification.NewIndex) + modification.NewItems.Count;
                    var startIndexOfAffectedItems = Math.Min(modification.OldIndex, modification.NewIndex);
                    var affectedItems = itemCollection.Skip(startIndexOfAffectedItems).Take(amountOfAffectedItems);
                    replaceItemIndexes(affectedItems, startIndexOfAffectedItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    itemIndexByKeyDictionary.Clear();
                    break;
            }
        }
    }
}
