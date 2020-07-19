using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Extensions.Collections;

namespace Teronis.Collections.Generic
{
    public class NullableKeyDictionary<KeyType, ValueType> : INullableKeyDictionary<KeyType, ValueType>, IReadOnlyNullableKeyDictionary<KeyType, ValueType>,
        IReadOnlyCollection<KeyValuePair<INullableKey<KeyType>, ValueType>>
        where KeyType : notnull
    {
        public ICollection<NullableKey<KeyType>> Keys {
            get {
                var readOnlyDictionary = (IReadOnlyDictionary<NullableKey<KeyType>, ValueType>)this;
                var keyList = new List<NullableKey<KeyType>>(readOnlyDictionary.Keys);
                return keyList.AsReadOnly();
            }
        }

        public ICollection<ValueType> Values {
            get {
                var readOnlyDictionary = (IReadOnlyDictionary<NullableKey<KeyType>, ValueType>)this;
                var valueList = new List<ValueType>(readOnlyDictionary.Values);
                return valueList.AsReadOnly();
            }
        }

        public int Count {
            get {
                var count = dictionary.Count;

                if (nullableKeyValuePair.HasValue) {
                    count++;
                }

                return count;
            }
        }

        public bool IsReadOnly => dictionary.AsCollectionWithPairs().IsReadOnly;

        private readonly Dictionary<KeyType, ValueType> dictionary;
        private KeyValuePair<NullableKey<KeyType>, ValueType>? nullableKeyValuePair;

        public NullableKeyDictionary() =>
            dictionary = new Dictionary<KeyType, ValueType>();

        public NullableKeyDictionary(IDictionary<KeyType, ValueType> dictionary) =>
            this.dictionary = new Dictionary<KeyType, ValueType>(dictionary);

        public NullableKeyDictionary(IDictionary<KeyType, ValueType> dictionary, IEqualityComparer<KeyType>? comparer) =>
            this.dictionary = new Dictionary<KeyType, ValueType>(dictionary, comparer);

        public NullableKeyDictionary(IEnumerable<KeyValuePair<KeyType, ValueType>> collection) =>
            dictionary = collection.ToDictionary(x => x.Key, x => x.Value);

        public NullableKeyDictionary(IEnumerable<KeyValuePair<KeyType, ValueType>> collection, IEqualityComparer<KeyType>? comparer) =>
            dictionary = collection.ToDictionary(x => x.Key, x => x.Value, comparer);

        public NullableKeyDictionary(IEqualityComparer<KeyType>? comparer) =>
            dictionary = new Dictionary<KeyType, ValueType>(comparer);

        public NullableKeyDictionary(int capacity) =>
            dictionary = new Dictionary<KeyType, ValueType>(capacity);

        public NullableKeyDictionary(int capacity, IEqualityComparer<KeyType>? comparer) =>
            dictionary = new Dictionary<KeyType, ValueType>(capacity, comparer);

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
                if (IsReadOnly) {
                    throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
                }

                if (key is null) {
                    nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(NullableKey<KeyType>.Null, value);
                } else {
                    dictionary[key] = value;
                }
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
                if (IsReadOnly) {
                    throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
                }

                if (key.IsNull) {
                    nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(key, value);
                } else {
                    dictionary[key] = value;
                }
            }
        }

        public void Add([AllowNull] KeyType key, ValueType value)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            if (key is null) {
                if (nullableKeyValuePair.HasValue) {
                    throw new ArgumentException();
                }

                nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(NullableKey<KeyType>.Null, value);
            } else {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="INullableKeyDictionary{KeyType, ValueType}"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(NullableKey<KeyType> key, [AllowNull] ValueType value)
        {
            if (IsReadOnly) {
                throw new NotSupportedException("");
            }

            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    throw new ArgumentException();
                }

                nullableKeyValuePair = new KeyValuePair<NullableKey<KeyType>, ValueType>(key, value!);
            } else {
                dictionary.Add(key, value!);
            }
        }

        public void Add([AllowNull] ValueType value) =>
            Add(NullableKey.Null<KeyType>(), value);

        bool ICollection<KeyValuePair<KeyType, ValueType>>.Contains(KeyValuePair<KeyType, ValueType> item) =>
            dictionary.AsCollectionWithPairs().Contains(item);

        public bool ContainsKey([AllowNull] KeyType key)
        {
            if (key is null) {
                return nullableKeyValuePair.HasValue;
            }

            return dictionary.ContainsKey(key);
        }

        public bool ContainsKey(NullableKey<KeyType> key)
        {
            if (key.IsNull) {
                return nullableKeyValuePair.HasValue;
            }

            return dictionary.ContainsKey(key);
        }

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

        public bool Remove([AllowNull] KeyType key)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            if (key is null) {
                if (nullableKeyValuePair.HasValue) {
                    nullableKeyValuePair = null;
                    return true;
                }

                return false;
            }

            return dictionary.Remove(key);
        }

        public bool Remove(NullableKey<KeyType> key)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    nullableKeyValuePair = null;
                    return true;
                }

                return false;
            }

            return dictionary.Remove(key);
        }

        public bool Remove() =>
            Remove(NullableKey<KeyType>.Null);

        public void Clear()
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            nullableKeyValuePair = null;
            dictionary.Clear();
        }

        public IEnumerator<KeyValuePair<NullableKey<KeyType>, ValueType>> GetEnumerator() =>
            new NullableKeyEnumuerator<KeyType, ValueType>(dictionary.AsCollectionWithPairs().GetEnumerator(), nullableKeyValuePair);

        public void CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            if (nullableKeyValuePair.HasValue) {
                array[arrayIndex++] = new KeyValuePair<KeyType, ValueType>(default!, nullableKeyValuePair.Value.Value);
            }

            dictionary.AsCollectionWithPairs().CopyTo(array, arrayIndex);
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

        #region IDictionary<KeyType, ValueType>

        ICollection<KeyType> IDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        ICollection<ValueType> IDictionary<KeyType, ValueType>.Values => dictionary.Values;

        bool IDictionary<KeyType, ValueType>.TryGetValue(KeyType key, [MaybeNullWhen(false)] out ValueType value) =>
            TryGetValue(key, out value);

        #endregion

        #region IReadOnlyDictionary<KeyType, ValueType>

        IEnumerable<KeyType> IReadOnlyDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        IEnumerable<ValueType> IReadOnlyDictionary<KeyType, ValueType>.Values => dictionary.Values;

        #endregion

        #region ICollection<KeyValuePair<KeyType, ValueType>>

        int ICollection<KeyValuePair<KeyType, ValueType>>.Count => dictionary.Count;

        void ICollection<KeyValuePair<KeyType, ValueType>>.Add(KeyValuePair<KeyType, ValueType> item)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            dictionary.AsCollectionWithPairs().Add(item);
        }

        bool ICollection<KeyValuePair<KeyType, ValueType>>.Remove(KeyValuePair<KeyType, ValueType> item)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            return dictionary.AsCollectionWithPairs().Remove(item);
        }

        #endregion

        #region IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>

        int IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>.Count => dictionary.Count;

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() =>
            this.AsReadOnlyDictionaryWithNullableKeys().GetEnumerator();

        #endregion

        #region IEnumerable<KeyValuePair<KeyType, ValueType>>

        IEnumerator<KeyValuePair<KeyType, ValueType>> IEnumerable<KeyValuePair<KeyType, ValueType>>.GetEnumerator() =>
            dictionary.AsCollectionWithPairs().GetEnumerator();

        #endregion

        #region IDictionary<NullableKey<KeyType>, ValueType>

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

            return dictionary.AsCollectionWithPairs().Contains(new KeyValuePair<KeyType, ValueType>(nullableKey, value));
        }

        bool ICollection<KeyValuePair<NullableKey<KeyType>, ValueType>>.Remove(KeyValuePair<NullableKey<KeyType>, ValueType> item)
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

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

        #region IReadOnlyDictionary<NullableKey<KeyType>, ValueType>

        IEnumerable<NullableKey<KeyType>> IReadOnlyDictionary<NullableKey<KeyType>, ValueType>.Keys {
            get {
                var nullableKeyValuePair = this.nullableKeyValuePair;

                if (nullableKeyValuePair != null) {
                    yield return nullableKeyValuePair.Value.Key;
                }

                foreach (var key in dictionary.Keys) {
                    yield return new NullableKey<KeyType>(key, false);
                }
            }
        }

        IEnumerable<ValueType> IReadOnlyDictionary<NullableKey<KeyType>, ValueType>.Values {
            get {
                var nullableKeyValuePair = this.nullableKeyValuePair;

                if (nullableKeyValuePair != null) {
                    yield return nullableKeyValuePair.Value.Value;
                }

                foreach (var value in dictionary.Values) {
                    yield return value;
                }
            }
        }

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

        #endregion

        #region IReadOnlyCollection<KeyValuePair<INullableKey<KeyType>,ValueType>>

        IEnumerator<KeyValuePair<INullableKey<KeyType>, ValueType>> IEnumerable<KeyValuePair<INullableKey<KeyType>, ValueType>>.GetEnumerator() =>
            new KeyValuePairEnumeratorWithPairHavingCovariantNullableKey<KeyType, ValueType>(GetEnumerator());

        #endregion

        internal static class NullableKeyDictionaryExceptionHelper
        {
            public static NotSupportedException CreateNotSupportedException() =>
                new NotSupportedException($"The {nameof(IReadOnlyNullableKeyDictionary<KeyType, ValueType>)} is read-only.");
        }
    }
}
