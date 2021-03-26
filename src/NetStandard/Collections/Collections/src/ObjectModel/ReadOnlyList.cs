// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Teronis.Collections.ObjectModel
{
    public class ReadOnlyList<ItemType> : IReadOnlyList<ItemType>, IList<ItemType>, IList
    {
        public virtual bool IsSynchronized => false;

        public int Count =>
            readOnlyList.Count;

        private readonly IReadOnlyList<ItemType> readOnlyList;

        private object? syncRoot;

        public ReadOnlyList(IReadOnlyList<ItemType> readOnlyList) =>
            this.readOnlyList = readOnlyList;

        public ItemType this[int index] => 
            readOnlyList[index];

        public IEnumerator<ItemType> GetEnumerator() => 
            readOnlyList.GetEnumerator();

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)readOnlyList).GetEnumerator();

        #endregion

        #region IList<ItemType>

        ItemType IList<ItemType>.this[int index] {
            get => readOnlyList[index];
            set => throw new NotSupportedException();
        }

        int IList<ItemType>.IndexOf(ItemType item)
        {
            var index = 0;

            foreach (var listItem in readOnlyList) {
                if (Equals(listItem, item)) {
                    return index;
                }

                index++;
            }

            return -1;
        }

        void IList<ItemType>.Insert(int index, ItemType item) => throw new NotSupportedException();
        void IList<ItemType>.RemoveAt(int index) => throw new NotSupportedException();

        bool ICollection<ItemType>.IsReadOnly => true;

        void ICollection<ItemType>.Add(ItemType item) => throw new NotSupportedException();
        void ICollection<ItemType>.Clear() => throw new NotSupportedException();
        bool ICollection<ItemType>.Contains(ItemType item) => readOnlyList.Contains(item);

        void ICollection<ItemType>.CopyTo(ItemType[] array, int arrayIndex) =>
            ((ICollection)this).CopyTo(array, arrayIndex);

        bool ICollection<ItemType>.Remove(ItemType item) => throw new NotSupportedException();

        #endregion

        #region IList

        bool IList.IsFixedSize => true;
        bool IList.IsReadOnly => true;

        object? IList.this[int index] {
            get => readOnlyList[index];
            set => throw new NotSupportedException();
        }

        int IList.Add(object? value) => throw new NotSupportedException();
        void IList.Clear() => throw new NotSupportedException();
        bool IList.Contains(object? value) => readOnlyList.Contains((ItemType)value);
        int IList.IndexOf(object? value) => throw new NotSupportedException();
        void IList.Insert(int index, object? value) => throw new NotSupportedException();
        void IList.Remove(object? value) => throw new NotSupportedException();
        void IList.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            foreach (var item in readOnlyList) {
                array.SetValue(item, arrayIndex++);
            }
        }

        object ICollection.SyncRoot {
            get {
                if (syncRoot == null) {
                    System.Threading.Interlocked.CompareExchange<object?>(ref syncRoot, new object(), null);
                }

                return syncRoot;
            }
        }

        #endregion
    }
}
