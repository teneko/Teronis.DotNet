using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public class KeyedItemIndexTracker<ItemType, KeyType>
    {
        public IReadOnlyDictionary<KeyType, IndexDirectoryEntry> KeyedItemIndexes { get; }

        internal readonly ISynchronizableItemCollection<ItemType> ItemCollection;
        internal readonly Func<ItemType, KeyType> GetItemKeyDelegate;

        private readonly Dictionary<KeyType, IndexDirectoryEntry> keyedItemIndexes;
        private readonly IndexDirectory indexDirectory;

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey,
            IEqualityComparer<KeyType>? keyEqualityComparer)
        {
            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<KeyType>.Default;
            keyedItemIndexes = new Dictionary<KeyType, IndexDirectoryEntry>(keyEqualityComparer);
            KeyedItemIndexes = new ReadOnlyDictionary<KeyType, IndexDirectoryEntry>(keyedItemIndexes);
            indexDirectory = new IndexDirectory();
            ItemCollection = itemCollection;
            GetItemKeyDelegate = getItemKey ?? throw new ArgumentNullException(nameof(getItemKey));
            itemCollection.CollectionModified += ModificationNotifier_CollectionModifying;
            //recalculateItemKeys();

            if (keyedItemIndexes.Count != 0) {
                keyedItemIndexes.Clear();
            }

            var itemCollectionCount = ItemCollection.Count;

            for (var index = 0; index < itemCollectionCount; index++) {
                var item = ItemCollection[index];
                var itemKey = GetItemKeyDelegate(item);
                var indexEntry = indexDirectory.Insert(index);
                keyedItemIndexes[itemKey] = indexEntry;
            }
        }

        //private void recalculateItemKeys()
        //{
            
        //}

        public KeyedItemIndexTracker(ISynchronizableItemCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey)
            : this(itemCollection, getItemKey, default(IEqualityComparer<KeyType>)) { }

        private void ModificationNotifier_CollectionModifying(object sender, ICollectionModification<ItemType, ItemType> modification)
        {
            //void replaceItemIndexes(IEnumerable<ItemType> items, int offset)
            //{
            //    var itemsEnumerator = items.GetEnumerator();
            //    var index = 0;

            //    //while (itemsEnumerator.MoveNext()) {
            //    //    var itemKey = GetItemKeyDelegate(itemsEnumerator.Current);
            //    //    indexDirectory.
            //    //    keyedItemIndexes[itemKey] = ;
            //    //    index++;
            //    //}
            //}

            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (modification.NewItems is null) {
                        throw CollectionModificationThrowHelper.NewItemsWereNullException();
                    }

                    var modificationNewIndex = modification.NewIndex;
                    var itemEnumerator = modification.NewItems.GetEnumerator();
                    var offset = 0;

                    while (itemEnumerator.MoveNext()) {
                        var itemKey = GetItemKeyDelegate(itemEnumerator.Current);
                        var indexEntry = indexDirectory.Insert(modificationNewIndex + offset);
                        keyedItemIndexes[itemKey] = indexEntry;
                        offset++;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove: {
                        if (modification.OldItems is null) {
                            throw CollectionModificationThrowHelper.OldItemsWereNullException();
                        }

                        var oldItemsCount = modification.OldItems.Count;

                        for (var index = 0; index < oldItemsCount; index++) {
                            var itemKey = GetItemKeyDelegate(modification.OldItems[index]);
                            var indexEntry = keyedItemIndexes[itemKey];
                            indexDirectory.RemoveEntry(indexEntry);
                            keyedItemIndexes.Remove(itemKey);
                        }

                        //recalculateItemKeys();
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Replace has not affect on calculated keys.
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (modification.OldItems is null) {
                        throw CollectionModificationThrowHelper.OldItemsWereNullException();
                    }

                    //var (StartIndexOfAffectedItems, AmountOfAffectedItems) = CollectionTools.GetMoveRange(
                    //    modification.OldIndex, 
                    //    modification.NewIndex, 
                    //    modification.NewItems.Count);

                    //var affectedItems = ItemCollection.Skip(StartIndexOfAffectedItems).Take(AmountOfAffectedItems);
                    //replaceItemIndexes(affectedItems, StartIndexOfAffectedItems);

                    indexDirectory.Move(modification.OldIndex, modification.NewIndex, modification.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    keyedItemIndexes.Clear();
                    indexDirectory.Clear();
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
            if (keyedItemIndexes.TryGetValue(key, out var itemIndex)) {
                var item = ItemCollection[itemIndex];
                return item;
            }

            return default;
        }
    }
}
