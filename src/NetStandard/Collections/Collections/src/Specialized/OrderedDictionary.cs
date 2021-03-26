// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Collections.Generic;
using Teronis.Collections.ObjectModel;

namespace Teronis.Collections.Specialized
{
    /// <summary>
    /// A dictionary object that allows rapid hash lookups using keys, but also
    /// maintains the key insertion order so that values can be retrieved by
    /// key index.
    /// </summary>
    /// <remarks>
    /// Similar to the way a DataColumn is indexed by column position and by column name, this
    /// advanced dictionary construct allows for a very natural and robust handling of indexed
    /// structured data.
    /// </remarks>
    // https://stackoverflow.com/a/9844528/11044059 (reference)
    public class OrderedDictionary<K, V> : IOrderedDictionary<K, V>
        where K : notnull
    {
        IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;
        IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

        private SortableKeyCollection<K, KeyValuePair<K, V>> keyedCollection = null!;

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to get or set.</param>
        public V this[K key] {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The index of the value to get or set.</param>
        public KeyValuePair<K, V> this[int index] {
            get => GetItem(index);
            set => SetItem(index, value.Value);
        }

        /// <summary>
        /// Gets the number of items in the dictionary
        /// </summary>
        public int Count => keyedCollection.Count;

        /// <summary>
        /// Gets all the keys in the ordered dictionary in their proper order.
        /// </summary>
        public ICollection<K> Keys => keyedCollection.Select(x => x.Key).ToList();

        /// <summary>
        /// Gets all the values in the ordered dictionary in their proper order.
        /// </summary>
        public ICollection<V> Values => keyedCollection.Select(x => x.Value).ToList();

        /// <summary>
        /// Gets the key comparer for this dictionary
        /// </summary>
        public IEqualityComparer<K>? Comparer { get; private set; }

        /* Constructors */

        public OrderedDictionary()
            => onConstruction();

        public OrderedDictionary(IEqualityComparer<K> comparer)
            => onConstruction(comparer);

        public OrderedDictionary(IOrderedDictionary<K, V> dictionary)
        {
            onConstruction();

            foreach (var pair in dictionary) {
                keyedCollection.Add(pair);
            }
        }

        public OrderedDictionary(IOrderedDictionary<K, V> dictionary, IEqualityComparer<K> comparer)
        {
            onConstruction(comparer);

            foreach (var pair in dictionary) {
                keyedCollection.Add(pair);
            }
        }

        public OrderedDictionary(IEnumerable<KeyValuePair<K, V>> items)
        {
            onConstruction();

            foreach (var pair in items) {
                keyedCollection.Add(pair);
            }
        }

        public OrderedDictionary(IEnumerable<KeyValuePair<K, V>> items, IEqualityComparer<K> comparer)
        {
            onConstruction(comparer);

            foreach (var pair in items) {
                keyedCollection.Add(pair);
            }
        }

        /* Methods */

        private void onConstruction(IEqualityComparer<K>? comparer = null)
        {
            Comparer = comparer;

            if (comparer != null) {
                keyedCollection = new SortableKeyCollection<K, KeyValuePair<K, V>>(x => x.Key, comparer);
            } else {
                keyedCollection = new SortableKeyCollection<K, KeyValuePair<K, V>>(x => x.Key);
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.  The value can be null for reference types.</param>
        public void Add(K key, [AllowNull] V value) =>
            keyedCollection.Add(new KeyValuePair<K, V>(key, value!));

        /// <summary>
        /// Removes all keys and values from this object.
        /// </summary>
        public void Clear() =>
            keyedCollection.Clear();

        public void Insert(int index, KeyValuePair<K, V> pair) =>
            keyedCollection.Insert(index, pair);

        /// <summary>
        /// Inserts a new key-value pair at the index specified.
        /// </summary>
        /// <param name="index">The insertion index.  This value must be between 0 and the count of items in this object.</param>
        /// <param name="key">A unique key for the element to add</param>
        /// <param name="value">The value of the element to add.  Can be null for reference types.</param>
        public void Insert(int index, K key, V value) => Insert(index, new KeyValuePair<K, V>(key, value));

        /// <summary>
        /// Gets the index of the key specified.
        /// </summary>
        /// <param name="key">The key whose index will be located</param>
        /// <returns>Returns the index of the key specified if found.  Returns -1 if the key could not be located.</returns>
        public int IndexOf(K key)
        {
            if (keyedCollection.Contains(key)) {
                return keyedCollection.IndexOf(keyedCollection[key]);
            } else {
                return -1;
            }
        }

        /// <summary>
        /// Determines whether this object contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this object.</param>
        /// <returns>True if the value is found.  False otherwise.</returns>
        public bool ContainsValue([AllowNull] V value) =>
            Values.Contains(value!);

        /// <summary>
        /// Determines whether this object contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this object.</param>
        /// <param name="comparer">The equality comparer used to locate the specified value in this object.</param>
        /// <returns>True if the value is found.  False otherwise.</returns>
        public bool ContainsValue([AllowNull] V value, IEqualityComparer<V> comparer) =>
            Values.Contains(value!, comparer);

        /// <summary>
        /// Determines whether this object contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in this object.</param>
        /// <returns>True if the key is found.  False otherwise.</returns>
        public bool ContainsKey(K key) =>
            keyedCollection.Contains(key);

        /// <summary>
        /// Returns the KeyValuePair at the index specified.
        /// </summary>
        /// <param name="index">The index of the KeyValuePair desired</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the index specified does not refer to a KeyValuePair in this object
        /// </exception>
        public KeyValuePair<K, V> GetItem(int index)
        {
            if (index < 0 || index >= keyedCollection.Count) {
                throw new ArgumentException("The index was outside the bounds of the dictionary: " + index);
            }

            return keyedCollection[index];
        }

        /// <summary>
        /// Sets the value at the index specified.
        /// </summary>
        /// <param name="index">The index of the value desired</param>
        /// <param name="value">The value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the index specified does not refer to a KeyValuePair in this object
        /// </exception>
        public void SetItem(int index, [AllowNull] V value)
        {
            if (index < 0 || index >= keyedCollection.Count) {
                throw new ArgumentException("The index is outside the bounds of the dictionary: " + index);
            }

            keyedCollection[index] = new KeyValuePair<K, V>(keyedCollection[index].Key, value!);
        }

        /// <summary>
        /// Returns an enumerator that iterates through all the KeyValuePairs in this object.
        /// </summary>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() =>
            keyedCollection.GetEnumerator();

        /// <summary>
        /// Removes the key-value pair for the specified key.
        /// </summary>
        /// <param name="key">The key to remove from the dictionary.</param>
        /// <returns>True if the item specified existed and the removal was successful.  False otherwise.</returns>
        public bool Remove(K key) => keyedCollection.Remove(key);

        /// <summary>
        /// Removes the key-value pair at the specified index.
        /// </summary>
        /// <param name="index">The index of the key-value pair to remove from the dictionary.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= keyedCollection.Count) {
                throw new ArgumentException("The index was outside the bounds of the dictionary: " + index);
            }

            keyedCollection.RemoveAt(index);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to get.</param>
        public V GetValue(K key)
        {
            if (keyedCollection.Contains(key) == false) {
                throw new ArgumentException("The given key is not present in the dictionary: " + key);
            }

            var kvp = keyedCollection[key];
            return kvp.Value;
        }

        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to set.</param>
        /// <param name="value">The the value to set.</param>
        public void SetValue(K key, [AllowNull] V value)
        {
            var kvp = new KeyValuePair<K, V>(key, value!);
            var idx = IndexOf(key);

            if (idx > -1) {
                keyedCollection[idx] = kvp;
            } else {
                keyedCollection.Add(kvp);
            }
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the desired element.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key if
        /// that key was found.  Otherwise it will contain the default value for parameter's type.
        /// This parameter should be provided uninitialized.
        /// </param>
        /// <returns>True if the value was found.  False otherwise.</returns>
        /// <remarks></remarks>
        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            if (keyedCollection.Contains(key)) {
                value = keyedCollection[key].Value;
                return true;
            } else {
                value = default;
                return false;
            }
        }

        public ReadOnlyDictionary<K, V> AsReadOnly() =>
            new ReadOnlyDictionary<K, V>(this);

        /* Sorting */
        public void SortKeys() => keyedCollection.SortByKeys();
        public void SortKeys(IComparer<K> comparer) => keyedCollection.SortByKeys(comparer);
        public void SortKeys(Comparison<K> comparison) => keyedCollection.SortByKeys(comparison);

        public void SortValues()
        {
            var comparer = Comparer<V>.Default;
            SortValues(comparer);
        }

        public void SortValues(IComparer<V> comparer) => keyedCollection.Sort((x, y) => comparer.Compare(x.Value, y.Value));
        public void SortValues(Comparison<V> comparison) => keyedCollection.Sort((x, y) => comparison(x.Value, y.Value));

        #region IDictionary<TKey, TValue>

        void IDictionary<K, V>.Add(K key, V value) => Add(key, value);
        bool IDictionary<K, V>.ContainsKey(K key) => ContainsKey(key);
        ICollection<K> IDictionary<K, V>.Keys => Keys;
        bool IDictionary<K, V>.Remove(K key) => Remove(key);
        bool IDictionary<K, V>.TryGetValue(K key, out V value) => TryGetValue(key, out value!);
        ICollection<V> IDictionary<K, V>.Values => Values;

        V IDictionary<K, V>.this[K key] {
            get => this[key]!;
            set => this[key] = value;
        }

        #endregion

        #region IReadOnlyDictionary<TKey, TValue>

        V IReadOnlyDictionary<K, V>.this[K key] {
            get => GetValue(key)!;
        }

        bool IReadOnlyDictionary<K, V>.TryGetValue(K key, out V value) =>
            TryGetValue(key, out value!);

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>>

        void ICollection<KeyValuePair<K, V>>.Add(KeyValuePair<K, V> item) => keyedCollection.Add(item);
        void ICollection<KeyValuePair<K, V>>.Clear() => keyedCollection.Clear();
        bool ICollection<KeyValuePair<K, V>>.Contains(KeyValuePair<K, V> item) => keyedCollection.Contains(item);
        void ICollection<KeyValuePair<K, V>>.CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) => keyedCollection.CopyTo(array, arrayIndex);
        int ICollection<KeyValuePair<K, V>>.Count => keyedCollection.Count;
        bool ICollection<KeyValuePair<K, V>>.IsReadOnly => false;
        bool ICollection<KeyValuePair<K, V>>.Remove(KeyValuePair<K, V> item) => keyedCollection.Remove(item);

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>>

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() =>
            GetEnumerator();

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        #endregion

        #region IOrderedDictionary

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() =>
            new DictionaryEnumerator<K, V>(this);

        void IOrderedDictionary.Insert(int index, object key, object? value) =>
            Insert(index, (K)key, (V)value!);

        void IOrderedDictionary.RemoveAt(int index) =>
            RemoveAt(index);

        object? IOrderedDictionary.this[int index] {
            get => this[index];
            set => this[index] = (KeyValuePair<K, V>)value!;
        }

        #endregion

        #region IDictionary

        void IDictionary.Add(object key, object? value) =>
            Add((K)key, (V)value);

        void IDictionary.Clear() =>
            Clear();

        bool IDictionary.Contains(object key) =>
            keyedCollection.Contains((K)key);

        IDictionaryEnumerator IDictionary.GetEnumerator() =>
            new DictionaryEnumerator<K, V>(this);

        bool IDictionary.IsFixedSize => false;
        bool IDictionary.IsReadOnly => false;
        ICollection IDictionary.Keys =>
            (ICollection)Keys;

        void IDictionary.Remove(object key) =>
            Remove((K)key);

        ICollection IDictionary.Values =>
            (ICollection)Values;

        object? IDictionary.this[object key] {
            get => this[(K)key]!;
            set => this[(K)key] = (V)value!;
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index) => ((ICollection)keyedCollection).CopyTo(array, index);
        int ICollection.Count => ((ICollection)keyedCollection).Count;
        bool ICollection.IsSynchronized => ((ICollection)keyedCollection).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)keyedCollection).SyncRoot;

        #endregion

    }
}
