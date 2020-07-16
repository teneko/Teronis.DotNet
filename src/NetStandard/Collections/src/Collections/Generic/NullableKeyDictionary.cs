using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class NullableKeyDictionary<KeyType, ValueType> : IDictionary<KeyType, ValueType>, IReadOnlyDictionary<KeyType, ValueType>,
        IDictionary<NullableKey<KeyType>, ValueType>, IReadOnlyDictionary<NullableKey<KeyType>, ValueType>,
        IReadOnlyCollection<KeyValuePair<INullableKey<KeyType>, ValueType>>
        where KeyType : notnull
    {
        public ICollection<KeyType> Keys => dictionary.Keys;
        IEnumerable<KeyType> IReadOnlyDictionary<KeyType, ValueType>.Keys => Keys;

        public ICollection<ValueType> Values => dictionary.Values;
        IEnumerable<ValueType> IReadOnlyDictionary<KeyType, ValueType>.Values => Values;

        public int Count {
            get {
                var count = dictionary.Count;

                if (nullableKeyValuePair.HasValue) {
                    count++;
                }

                return count;
            }
        }

        public bool IsReadOnly => dictionaryAsColletion.IsReadOnly;

        private Dictionary<KeyType, ValueType> dictionary;
        private ICollection<KeyValuePair<KeyType, ValueType>> dictionaryAsColletion;
        private KeyValuePair<NullableKey<KeyType>, ValueType>? nullableKeyValuePair;

        public NullableKeyDictionary()
        {
            dictionary = new Dictionary<KeyType, ValueType>();
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(IDictionary<KeyType, ValueType> dictionary)
        {
            this.dictionary = new Dictionary<KeyType, ValueType>(dictionary);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(IDictionary<KeyType, ValueType> dictionary, IEqualityComparer<KeyType>? comparer)
        {
            this.dictionary = new Dictionary<KeyType, ValueType>(dictionary, comparer);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(IEnumerable<KeyValuePair<KeyType, ValueType>> collection)
        {
            dictionary = new Dictionary<KeyType, ValueType>(collection);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(IEnumerable<KeyValuePair<KeyType, ValueType>> collection, IEqualityComparer<KeyType>? comparer)
        {
            dictionary = new Dictionary<KeyType, ValueType>(collection, comparer);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(IEqualityComparer<KeyType>? comparer)
        {
            dictionary = new Dictionary<KeyType, ValueType>(comparer);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(int capacity)
        {
            dictionary = new Dictionary<KeyType, ValueType>(capacity);
            dictionaryAsColletion = dictionary;
        }

        public NullableKeyDictionary(int capacity, IEqualityComparer<KeyType>? comparer)
        {
            dictionary = new Dictionary<KeyType, ValueType>(capacity, comparer);
            dictionaryAsColletion = dictionary;
        }

        public ValueType this[[AllowNull] KeyType key] {
            get {
                if (key is null) {
                    if (!nullableKeyValuePair.HasValue) {
                        throw new KeyNotFoundException();
                    }

                    return nullableKeyValuePair.Value.Value;
                }

                return dictionary[key];
            }

            set {
                if (key is null) {
                    nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(NullableKey<KeyType>.Null, value);
                } else {
                    dictionary[key] = value;
                }
            }
        }

        public void Add([AllowNull] KeyType key, ValueType value)
        {
            if (key is null) {
                if (nullableKeyValuePair.HasValue) {
                    throw new ArgumentException();
                }

                nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(NullableKey<KeyType>.Null, value);
            } else {
                dictionary.Add(key, value);
            }
        }

        void ICollection<KeyValuePair<KeyType, ValueType>>.Add(KeyValuePair<KeyType, ValueType> item) =>
            dictionaryAsColletion.Add(item);

        public void Clear()
        {
            nullableKeyValuePair = null;
            dictionary.Clear();
        }

        bool ICollection<KeyValuePair<KeyType, ValueType>>.Contains(KeyValuePair<KeyType, ValueType> item) =>
            dictionaryAsColletion.Contains(item);

        public bool ContainsKey([AllowNull] KeyType key)
        {
            if (key is null) {
                return nullableKeyValuePair.HasValue;
            }

            return dictionary.ContainsKey(key);
        }

        void ICollection<KeyValuePair<KeyType, ValueType>>.CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            if (nullableKeyValuePair.HasValue) {
                array[arrayIndex++] = new KeyValuePair<KeyType, ValueType>(default!, nullableKeyValuePair.Value.Value);
            }

            dictionaryAsColletion.CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<KeyType, ValueType>> IEnumerable<KeyValuePair<KeyType, ValueType>>.GetEnumerator() =>
            dictionaryAsColletion.GetEnumerator();

        public bool Remove([AllowNull] KeyType key)
        {
            if (key is null) {
                if (nullableKeyValuePair.HasValue) {
                    nullableKeyValuePair = null;
                    return true;
                }

                return false;
            }

            return dictionary.Remove(key);
        }

        bool ICollection<KeyValuePair<KeyType, ValueType>>.Remove(KeyValuePair<KeyType, ValueType> item) =>
            dictionaryAsColletion.Remove(item);

        public bool TryGetValue([AllowNull] KeyType key, [MaybeNullWhen(false)] out ValueType value)
        {
            if (key is null) {
                if (nullableKeyValuePair.HasValue) {
                    value = nullableKeyValuePair.Value.Value;
                    return true;
                }

                value = default;
                return false;
            } else {
                return dictionary.TryGetValue(key, out value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            dictionary.GetEnumerator();

        #region IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>

        int IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>.Count => dictionary.Count;

        #endregion

        #region IReadOnlyDictionary<NullableKey<KeyType>, ValueType>

        IEnumerable<NullableKey<KeyType>> IReadOnlyDictionary<NullableKey<KeyType>, ValueType>.Keys {
            get {
                var nullableKeyValuePair = this.nullableKeyValuePair;

                if (nullableKeyValuePair != null) {
                    yield return nullableKeyValuePair.Value.Key;
                }

                foreach (var key in Keys) {
                    yield return new NullableKey<KeyType>(key, false);
                }
            }
        }

        IEnumerable<ValueType> IReadOnlyDictionary<NullableKey<KeyType>, ValueType>.Values => Values;

        ValueType IReadOnlyDictionary<NullableKey<KeyType>, ValueType>.this[NullableKey<KeyType> key] {
            get {
                if (key.IsNull) {
                    if (!nullableKeyValuePair.HasValue) {
                        throw new KeyNotFoundException("The key does not exist in the collection.");
                    }

                    return nullableKeyValuePair.Value.Value;
                }

                return dictionary[key];
            }
        }

        public bool ContainsKey(NullableKey<KeyType> key)
        {
            if (key.IsNull) {
                return nullableKeyValuePair.HasValue;
            }

            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(NullableKey<KeyType> key, [MaybeNullWhen(false)] out ValueType value)
        {
            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    value = nullableKeyValuePair.Value.Value;
                    return true;
                }
            } else {
                return dictionary.TryGetValue(key, out value);
            }

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<NullableKey<KeyType>, ValueType>> GetEnumerator() =>
            new NullableKeyEnumuerator<KeyType, ValueType>(dictionaryAsColletion.GetEnumerator(), nullableKeyValuePair);

        #endregion

        #region IDictionary<NullableKey<KeyType>, ValueType>

        ICollection<NullableKey<KeyType>> IDictionary<NullableKey<KeyType>, ValueType>.Keys {
            get {
                var list = new List<NullableKey<KeyType>>(((IReadOnlyDictionary<NullableKey<KeyType>, ValueType>)this).Keys);

                if (nullableKeyValuePair.HasValue) {
                    list.Insert(0, nullableKeyValuePair.Value.Key);
                }

                return list;
            }
        }

        public ValueType this[NullableKey<KeyType> key] {
            get {
                if (key.IsNull) {
                    if (!nullableKeyValuePair.HasValue) {
                        throw new KeyNotFoundException();
                    }

                    return nullableKeyValuePair.Value.Value;
                }

                return dictionary[key];
            }

            set {
                if (key.IsNull) {
                    nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(key, value);
                } else {
                    dictionary[key] = value;
                }
            }
        }

        public void Add(NullableKey<KeyType> key, ValueType value)
        {
            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    throw new ArgumentException();
                }

                nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(key, value);
            } else {
                dictionary.Add(key, value);
            }
        }

        public void Add(NullableKey key, ValueType value) =>
            Add(NullableKey.Null<KeyType>(), value);

        public bool Remove(NullableKey<KeyType> key)
        {
            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    nullableKeyValuePair = null;
                    return true;
                }

                return false;
            }

            return dictionary.Remove(key);
        }

        void ICollection<KeyValuePair<NullableKey<KeyType>, ValueType>>.Add(KeyValuePair<NullableKey<KeyType>, ValueType> item) =>
            Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<NullableKey<KeyType>, ValueType>>.Contains(KeyValuePair<NullableKey<KeyType>, ValueType> item)
        {
            var (nullableKey, value) = item;

            if (nullableKey.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    return EqualityComparer<ValueType>.Default.Equals(item.Value, nullableKeyValuePair.Value.Value);
                }

                return false;
            }

            return dictionaryAsColletion.Contains(new KeyValuePair<KeyType, ValueType>(nullableKey, value));
        }

        public void CopyTo(KeyValuePair<NullableKey<KeyType>, ValueType>[] array, int arrayIndex)
        {
            if (nullableKeyValuePair.HasValue) {
                array[arrayIndex++] = nullableKeyValuePair.Value;
            }

            var readOnlyDictionary = (IReadOnlyDictionary<NullableKey<KeyType>, ValueType>)this;

            foreach (var pair in readOnlyDictionary) {
                array[arrayIndex++] = pair;
            }
        }

        bool ICollection<KeyValuePair<NullableKey<KeyType>, ValueType>>.Remove(KeyValuePair<NullableKey<KeyType>, ValueType> item)
        {
            var (nullableKey, value) = item;

            if (TryGetValue(nullableKey, out var foundValue)) {
                if (EqualityComparer<ValueType>.Default.Equals(value, foundValue)) {
                    Remove(nullableKey);
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region IReadOnlyCollection<KeyValuePair<INullableKey<KeyType>,ValueType>>

        IEnumerator<KeyValuePair<INullableKey<KeyType>, ValueType>> IEnumerable<KeyValuePair<INullableKey<KeyType>, ValueType>>.GetEnumerator() =>
            new KeyValuePairEnumeratorWithPairHavingCovariantNullableKey<KeyType, ValueType>(GetEnumerator());

        #endregion
    }
}
