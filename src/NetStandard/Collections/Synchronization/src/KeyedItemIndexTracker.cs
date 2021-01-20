using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Collections.Algorithms;
using Teronis.Tools;

namespace Teronis.Collections.Synchronization
{
    public class KeyedItemIndexTracker<ItemType, KeyType>
    {
        public IReadOnlyDictionary<KeyType, int> KeyedItemIndexes { get; }

        internal readonly ISynchronizableItemCollection<ItemType> ItemCollection;
        internal readonly Func<ItemType, KeyType> GetItemKeyDelegate;

        private readonly Dictionary<KeyType, int> keyedItemIndexes;

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey,
            IEqualityComparer<KeyType>? keyEqualityComparer)
        {
            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<KeyType>.Default;
            keyedItemIndexes = new Dictionary<KeyType, int>(keyEqualityComparer);
            KeyedItemIndexes = new ReadOnlyDictionary<KeyType, int>(keyedItemIndexes);
            ItemCollection = itemCollection;
            GetItemKeyDelegate = getItemKey ?? throw new ArgumentNullException(nameof(getItemKey));
            itemCollection.CollectionModified += ModificationNotifier_CollectionModified;
            recalculateItemKeys();
        }

        private void recalculateItemKeys()
        {
            if (keyedItemIndexes.Count != 0) {
                keyedItemIndexes.Clear();
            }

            var itemCollectionCount = ItemCollection.Count;

            for (var index = 0; index < itemCollectionCount; index++) {
                var item = ItemCollection[index];
                var itemKey = GetItemKeyDelegate(item);
                keyedItemIndexes[itemKey] = index;
            }
        }

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey)
            : this(itemCollection, getItemKey, default(IEqualityComparer<KeyType>)) { }

        private void ModificationNotifier_CollectionModified(object sender, ICollectionModification<ItemType, ItemType> modification)
        {
            void replaceItemIndexes(IEnumerable<ItemType> items, int offset)
            {
                var itemsEnumerator = items.GetEnumerator();
                var index = 0;

                while (itemsEnumerator.MoveNext()) {
                    var itemKey = GetItemKeyDelegate(itemsEnumerator.Current);
                    keyedItemIndexes[itemKey] = offset + index;
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
                            var itemKey = GetItemKeyDelegate(modification.OldItems[index]);
                            keyedItemIndexes.Remove(itemKey);
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

                    var (StartIndexOfAffectedItems, AmountOfAffectedItems) = CollectionTools.GetMoveRange(
                        modification.OldIndex, 
                        modification.NewIndex, 
                        modification.NewItems.Count);

                    var affectedItems = ItemCollection.Skip(StartIndexOfAffectedItems).Take(AmountOfAffectedItems);
                    replaceItemIndexes(affectedItems, StartIndexOfAffectedItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    keyedItemIndexes.Clear();
                    break;
            }
        }

        public ItemType GetItem(KeyType key)
        {
            var itemIndex = keyedItemIndexes[key];
            var item = ItemCollection[itemIndex];
            return item;
        }

        [return: MaybeNull]
        public ItemType GetItemOrDefault(KeyType key)
        {
            if (keyedItemIndexes.TryGetValue(key, out int itemIndex)) {
                var item = ItemCollection[itemIndex];
                return item;
            }

            return default;
        }
    }
}
