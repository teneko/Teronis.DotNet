using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Teronis.NetStandard.Collections.Generic
{
    public class UniqueValueList<T> : IList<T>
    {
        const string EXCEPTION_ITEM_EXISTING = "Item already existing.";

        List<T> list;

        public UniqueValueList() => list = new List<T>();

        public T this[int index] { get => list[index]; set => list[index] = value; }
        public T this[T item] { get => list[list.IndexOf(item)]; set => list[list.IndexOf(item)] = value; }
        public int Count => list.Count;

        bool ICollection<T>.IsReadOnly => ((IList)list).IsReadOnly;

        public void Add(T item)
        {
            if (!list.Contains(item))
                list.Add(item);
            else
                throw new Exception(EXCEPTION_ITEM_EXISTING);
        }

        public bool TryAdd(T item)
        {
            if (!list.Contains(item)) {
                list.Add(item);
                return true;
            } else
                return false;
        }

        public void Clear() => list.Clear();
        public bool Contains(T item) => list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        public int IndexOf(T item) => list.IndexOf(item);

        public bool TryGetItem(T item, out T retVal)
        {
            var index = list.IndexOf(item);

            if (index != -1) {
                retVal = list[index];
                return true;
            } else {
                retVal = default;
                return false;
            }
        }

        public void Insert(int index, T item)
        {
            if (!list.Contains(item))
                list.Insert(index, item);

            else throw new Exception(EXCEPTION_ITEM_EXISTING);
        }

        public bool TryInsert(int index, T item)
        {
            if (!list.Contains(item)) {
                list.Insert(index, item);
                return true;
            } else
                return false;
        }

        public bool Remove(T item) => list.Remove(item);
        public void RemoveAt(int index) => list.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();
        public ReadOnlyCollection<T> AsReadOnly() => list.AsReadOnly();
    }
}
