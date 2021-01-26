using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizableItemCollection<ItemType> :
        ICollection<ItemType>, IEnumerable<ItemType>, IEnumerable, IList<ItemType>, IReadOnlyCollection<ItemType>, IReadOnlyList<ItemType>, ICollection, IList,
        INotifyCollectionSynchronizing<ItemType>, INotifyCollectionModification<ItemType>, INotifyCollectionChanged, INotifyCollectionSynchronized<ItemType>
    {
        new ItemType this[int index] { get; set; }
        new int Count { get; }
        new void Add(ItemType item);
        new void Clear();
        new bool Contains(ItemType item);
        new void CopyTo(ItemType[] array, int index);
        new IEnumerator<ItemType> GetEnumerator();
        new int IndexOf(ItemType item);
        new void Insert(int index, ItemType item);
        new bool Remove(ItemType item);
        new void RemoveAt(int index);

        KeyedItemIndexTracker<ItemType, KeyType> CreateKeyedItemIndexTracker<KeyType>(Func<ItemType, KeyType> getItemKey);
        KeyedItemIndexTracker<ItemType, KeyType> CreateKeyedItemIndexTracker<KeyType>(Func<ItemType, KeyType> getItemKey, IEqualityComparer<KeyType> keyEqualityComparer);
    }
}
