// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Collections.Algorithms.Modifications;
using Teronis.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public class SynchronizedDictionary<TKey, TItem> : IReadOnlyDictionary<TKey, TItem>
        where TKey : notnull
    {
        public IReadOnlyDictionary<TKey, IndexDirectoryEntry> KeyedIndexes { get; }

        public int Count =>
            KeyedIndexes.Count;

        internal readonly ISynchronizedCollection<TItem> ItemCollection;
        internal readonly Func<TItem, TKey> GetItemKeyDelegate;

        private readonly Dictionary<TKey, IndexDirectoryEntry> keyedIndexes;
        private readonly IndexDirectory indexDirectory;

        public SynchronizedDictionary(ISynchronizedCollection<TItem> itemCollection, Func<TItem, TKey> getItemKey,
            IEqualityComparer<TKey>? keyEqualityComparer)
        {
            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<TKey>.Default;
            keyedIndexes = new Dictionary<TKey, IndexDirectoryEntry>(keyEqualityComparer);
            KeyedIndexes = new ReadOnlyDictionary<TKey, IndexDirectoryEntry>(keyedIndexes);
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

        public SynchronizedDictionary(ISynchronizedCollection<TItem> itemCollection, Func<TItem, TKey> getItemKey)
            : this(itemCollection, getItemKey, default(IEqualityComparer<TKey>)) { }

        public TItem GetItem(TKey key)
        {
            var itemIndex = keyedIndexes[key];
            var item = ItemCollection[itemIndex];
            return item;
        }

        public TItem this[TKey key] =>
            GetItem(key);

        private void ModificationNotifier_CollectionModifying(object sender, ICollectionModification<TItem, TItem> modification)
        {
            switch (modification.Action) {
                case NotifyCollectionChangedAction.Add:
                    CollectionModificationIterationTools.BeginInsert(modification)
                        .OnIteration(addedItemIndex => {
                            var itemKey = GetItemKeyDelegate(modification.NewItems![addedItemIndex.ModificationItemIndex]);
                            var indexEntry = indexDirectory.Insert(addedItemIndex.CollectionItemIndex);
                            keyedIndexes.Add(itemKey, indexEntry);
                        })
                        .Iterate();

                    break;
                case NotifyCollectionChangedAction.Remove: {
                        CollectionModificationIterationTools.BeginRemove(modification)
                            .OnIteration(iterationContext => {
                                var itemKey = GetItemKeyDelegate(modification.OldItems![iterationContext.ModificationItemIndex]);
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

        public bool TryGetItem(TKey key, out TItem value)
        {
            if (keyedIndexes.TryGetValue(key, out var itemIndex)) {
                value = ItemCollection[itemIndex];
                return true;
            }

            value = default!;
            return false;
        }

        [return: MaybeNull]
        public TItem GetItemOrDefault(TKey key)
        {
            if (TryGetItem(key, out TItem item)) {
                return item;
            }

            return default;
        }

        public bool ContainsKey(TKey key) =>
            KeyedIndexes.ContainsKey(key);

        bool IReadOnlyDictionary<TKey, TItem>.TryGetValue(TKey key, out TItem value) =>
            TryGetItem(key, out value);

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TItem>.Keys =>
            KeyedIndexes.Keys;

        IEnumerable<TItem> IReadOnlyDictionary<TKey, TItem>.Values =>
            KeyedIndexes.Keys.Select(x => GetItem(x));

        public IEnumerator<KeyValuePair<TKey, TItem>> GetEnumerator() =>
            new ItemEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public class ItemEnumerator : IEnumerator<KeyValuePair<TKey, TItem>>
        {
            public KeyValuePair<TKey, TItem> Current => current;

            private readonly SynchronizedDictionary<TKey, TItem> synchronizedCollection;
            private IEnumerator<TKey>? keyEnumerator;
            private KeyValuePair<TKey, TItem> current;

            internal ItemEnumerator(SynchronizedDictionary<TKey, TItem> synchronizedCollection)
            {
                current = new KeyValuePair<TKey, TItem>();
                this.synchronizedCollection = synchronizedCollection;
            }

            public bool MoveNext()
            {
                if (keyEnumerator is null) {
                    keyEnumerator = synchronizedCollection.KeyedIndexes.Keys.GetEnumerator();
                }

                if (keyEnumerator.MoveNext()) {
                    var key = keyEnumerator.Current;

                    current = new KeyValuePair<TKey, TItem>(
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
                    current = new KeyValuePair<TKey, TItem>();
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
