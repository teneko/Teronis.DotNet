using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizedDictionary<KeyType, ItemType> : IReadOnlyDictionary<KeyType, ItemType>
        where KeyType : notnull
    {
        public IReadOnlyDictionary<KeyType, IndexDirectoryEntry> KeyedIndexes { get; }

        public int Count =>
            KeyedIndexes.Count;

        internal readonly ISynchronizedCollection<ItemType> ItemCollection;
        internal readonly Func<ItemType, KeyType> GetItemKeyDelegate;

        private readonly Dictionary<KeyType, IndexDirectoryEntry> keyedIndexes;
        private readonly IndexDirectory indexDirectory;

        public SynchronizedDictionary(ISynchronizedCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey,
            IEqualityComparer<KeyType>? keyEqualityComparer)
        {
            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<KeyType>.Default;
            keyedIndexes = new Dictionary<KeyType, IndexDirectoryEntry>(keyEqualityComparer);
            KeyedIndexes = new ReadOnlyDictionary<KeyType, IndexDirectoryEntry>(keyedIndexes);
            indexDirectory = new IndexDirectory();
            ItemCollection = itemCollection;
            GetItemKeyDelegate = getItemKey ?? throw new ArgumentNullException(nameof(getItemKey));
            itemCollection.CollectionModified += ModificationNotifier_CollectionModifying;

            if (keyedIndexes.Count != 0) {
                keyedIndexes.Clear();
            }

            var itemCollectionCount = ItemCollection.Count;

            for (var index = 0; index < itemCollectionCount; index++) {
                var item = ItemCollection[index];
                var itemKey = GetItemKeyDelegate(item);
                var indexEntry = indexDirectory.Insert(index);
                keyedIndexes[itemKey] = indexEntry;
            }
        }

        public SynchronizedDictionary(ISynchronizedCollection<ItemType> itemCollection, Func<ItemType, KeyType> getItemKey)
            : this(itemCollection, getItemKey, default(IEqualityComparer<KeyType>)) { }

        public ItemType GetItem(KeyType key)
        {
            var itemIndex = keyedIndexes[key];
            var item = ItemCollection[itemIndex];
            return item;
        }

        public ItemType this[KeyType key] =>
            GetItem(key);

        private void ModificationNotifier_CollectionModifying(object sender, ICollectionModification<ItemType, ItemType> modification)
        {
            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    CollectionModificationIterationTools.BeginInsert(modification)
                        .Add((frontIndex, backIndexOffset) => {
                            var itemKey = GetItemKeyDelegate(modification.NewItems![frontIndex]);
                            var indexEntry = indexDirectory.Insert(frontIndex + backIndexOffset);
                            keyedIndexes.Add(itemKey, indexEntry);
                        })
                        .Iterate();

                    break;
                case NotifyCollectionChangedAction.Remove: {
                        CollectionModificationIterationTools.BeginRemove(modification)
                            .Add((frontIndex, backIndexOffset) => {
                                var itemKey = GetItemKeyDelegate(modification.OldItems![frontIndex]);
                                var indexEntry = keyedIndexes[itemKey];
                                indexDirectory.RemoveEntry(indexEntry);
                                keyedIndexes.Remove(itemKey);
                            })
                            .Iterate();
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Replace has not affect on calculated keys.
                    break;
                case NotifyCollectionChangedAction.Move:
                    CollectionModificationIterationTools.CheckMove(modification);
                    indexDirectory.Move(modification.OldIndex, modification.NewIndex, modification.OldItems!.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    keyedIndexes.Clear();
                    indexDirectory.Clear();
                    break;
            }
        }

        public bool TryGetItem(KeyType key, out ItemType value)
        {
            if (keyedIndexes.TryGetValue(key, out var itemIndex)) {
                value = ItemCollection[itemIndex];
                return true;
            }

            value = default!;
            return false;
        }

        public ItemType GetItemOrDefault(KeyType key)
        {
            if (TryGetItem(key, out ItemType item)) {
                return item;
            }

            return default!;
        }

        public bool ContainsKey(KeyType key) =>
            KeyedIndexes.ContainsKey(key);

        bool IReadOnlyDictionary<KeyType, ItemType>.TryGetValue(KeyType key, out ItemType value) =>
            TryGetItem(key, out value);

        IEnumerable<KeyType> IReadOnlyDictionary<KeyType, ItemType>.Keys =>
            KeyedIndexes.Keys;

        IEnumerable<ItemType> IReadOnlyDictionary<KeyType, ItemType>.Values =>
            KeyedIndexes.Keys.Select(x => GetItem(x));

        public IEnumerator<KeyValuePair<KeyType, ItemType>> GetEnumerator() =>
            new ItemEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public class ItemEnumerator : IEnumerator<KeyValuePair<KeyType, ItemType>>
        {
            public KeyValuePair<KeyType, ItemType> Current => current;

            private readonly SynchronizedDictionary<KeyType, ItemType> synchronizedCollection;
            private IEnumerator<KeyType>? keyEnumerator;
            private KeyValuePair<KeyType, ItemType> current;

            internal ItemEnumerator(SynchronizedDictionary<KeyType, ItemType> synchronizedCollection)
            {
                current = new KeyValuePair<KeyType, ItemType>();
                this.synchronizedCollection = synchronizedCollection;
            }

            public bool MoveNext()
            {
                if (keyEnumerator is null) {
                    keyEnumerator = synchronizedCollection.KeyedIndexes.Keys.GetEnumerator();
                }

                if (keyEnumerator.MoveNext()) {
                    var key = keyEnumerator.Current;

                    current = new KeyValuePair<KeyType, ItemType>(
                        key,
                        synchronizedCollection.GetItem(keyEnumerator.Current));

                    return false;
                }

                current = default!;
                return false;
            }

            public void Reset()
            {
                if (keyEnumerator is null) {
                    keyEnumerator = synchronizedCollection.KeyedIndexes.Keys.GetEnumerator();
                } else {
                    current = new KeyValuePair<KeyType, ItemType>();
                    keyEnumerator.Reset();
                }
            }

            object? IEnumerator.Current =>
                Current;

            public void Dispose() =>
                keyEnumerator?.Dispose();
        }
    }
}
