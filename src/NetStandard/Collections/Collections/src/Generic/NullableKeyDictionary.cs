using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Teronis.Extensions;

namespace Teronis.Collections.Generic
{
    public class NullableKeyDictionary<KeyType, ValueType> : INullableKeyDictionary<KeyType, ValueType>, IReadOnlyNullableKeyDictionary<KeyType, ValueType>
        where KeyType : notnull
    {
        public ICollection<YetNullable<KeyType>> Keys {
            get {
                var readOnlyDictionary = (IReadOnlyDictionary<YetNullable<KeyType>, ValueType>)this;
                var keyList = new List<YetNullable<KeyType>>(readOnlyDictionary.Keys);
                return keyList.AsReadOnly();
            }
        }

        public ICollection<ValueType> Values {
            get {
                var readOnlyDictionary = (IReadOnlyDictionary<YetNullable<KeyType>, ValueType>)this;
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
        private KeyValuePair<YetNullable<KeyType>, ValueType>? nullableKeyValuePair;

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

        public ValueType this[YetNullable<KeyType> key] {
            get {
                if (key.IsNull) {
                    if (!nullableKeyValuePair.HasValue) {
                        throw new KeyNotFoundException();
                    }

                    return nullableKeyValuePair.Value.Value;
                }

                return dictionary[key.value];
            }

            set {
                if (IsReadOnly) {
                    throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
                }

                if (key.IsNull) {
                    nullableKeyValuePair = new KeyValuePair<YetNullable<KeyType>, ValueType>(key, value);
                } else {
                    dictionary[key.value] = value;
                }
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="INullableKeyDictionary{KeyType, ValueType}"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(YetNullable<KeyType> key, ValueType value)
        {
            if (IsReadOnly) {
                throw new NotSupportedException("");
            }

            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    throw new ArgumentException();
                }

                nullableKeyValuePair = new KeyValuePair<YetNullable<KeyType>, ValueType>(key, value!);
            } else {
                dictionary.Add(key.value, value!);
            }
        }

        public void Add(ValueType value) =>
            Add(YetNullable<KeyType>.Null, value);

        bool ICollection<KeyValuePair<KeyType, ValueType>>.Contains(KeyValuePair<KeyType, ValueType> item) =>
            dictionary.AsCollectionWithPairs().Contains(item);

        public bool ContainsKey(YetNullable<KeyType> key)
        {
            if (key.IsNull) {
                return nullableKeyValuePair.HasValue;
            }

            return dictionary.ContainsKey(key.value);
        }

        public bool TryGetValue(YetNullable<KeyType> key, [MaybeNullWhen(false)] out ValueType value)
        {
            if (key.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    value = nullableKeyValuePair.Value.Value;
                    return true;
                }
            } else {
                return dictionary.TryGetValue(key.value, out value);
            }

            value = default;
            return false;
        }

        public CovariantTuple<bool, ValueType> FindValue(YetNullable<KeyType> key)
        {
            if (TryGetValue(key, out ValueType value)) {
                return new CovariantTuple<bool, ValueType>(true, value);
            }

            return new CovariantTuple<bool, ValueType>(default, default!);
        }

        public bool Remove(YetNullable<KeyType> key)
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

            return dictionary.Remove(key.value);
        }

        public bool Remove() =>
            Remove(YetNullable<KeyType>.Null);

        public void Clear()
        {
            if (IsReadOnly) {
                throw NullableKeyDictionaryExceptionHelper.CreateNotSupportedException();
            }

            nullableKeyValuePair = null;
            dictionary.Clear();
        }

        public IEnumerator<KeyValuePair<YetNullable<KeyType>, ValueType>> GetEnumerator() =>
            new NullableKeyEnumuerator<KeyType, ValueType>(dictionary.AsCollectionWithPairs().GetEnumerator(), nullableKeyValuePair);

        public void CopyTo(KeyValuePair<KeyType, ValueType>[] array, int arrayIndex)
        {
            if (nullableKeyValuePair.HasValue) {
                array[arrayIndex++] = new KeyValuePair<KeyType, ValueType>(default!, nullableKeyValuePair.Value.Value);
            }

            dictionary.AsCollectionWithPairs().CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<YetNullable<KeyType>, ValueType>[] array, int arrayIndex)
        {
            if (nullableKeyValuePair.HasValue) {
                array[arrayIndex++] = nullableKeyValuePair.Value;
            }

            var readOnlyDictionary = (IReadOnlyDictionary<YetNullable<KeyType>, ValueType>)this;

            foreach (var pair in readOnlyDictionary) {
                array[arrayIndex++] = pair;
            }
        }

        #region IDictionary<KeyType, ValueType>

        ValueType IDictionary<KeyType, ValueType>.this[KeyType key] {
            get => this[key];
            set => this[key] = value;
        }

        ICollection<KeyType> IDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        ICollection<ValueType> IDictionary<KeyType, ValueType>.Values => dictionary.Values;

        void IDictionary<KeyType, ValueType>.Add(KeyType key, ValueType value) =>
            Add(key, value);

        bool IDictionary<KeyType, ValueType>.Remove(KeyType key) =>
            Remove(key);

        bool IDictionary<KeyType, ValueType>.ContainsKey(KeyType key) =>
            ContainsKey(key);

        bool IDictionary<KeyType, ValueType>.TryGetValue(KeyType key, [MaybeNullWhen(false)] out ValueType value) =>
            TryGetValue(key, out value);

        #endregion

        #region IReadOnlyDictionary<KeyType, ValueType>

        IEnumerable<KeyType> IReadOnlyDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        IEnumerable<ValueType> IReadOnlyDictionary<KeyType, ValueType>.Values => dictionary.Values;

        bool IReadOnlyDictionary<KeyType, ValueType>.TryGetValue(KeyType key, [MaybeNullWhen(false)] out ValueType value)
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

        #region IDictionary<YetNullable<KeyType>, ValueType>

        void ICollection<KeyValuePair<YetNullable<KeyType>, ValueType>>.Add(KeyValuePair<YetNullable<KeyType>, ValueType> item) =>
            Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<YetNullable<KeyType>, ValueType>>.Contains(KeyValuePair<YetNullable<KeyType>, ValueType> item)
        {
            var (nullableKey, value) = item;

            if (nullableKey.IsNull) {
                if (nullableKeyValuePair.HasValue) {
                    return EqualityComparer<ValueType>.Default.Equals(item.Value, nullableKeyValuePair.Value.Value);
                }

                return false;
            }

            return dictionary.AsCollectionWithPairs().Contains(new KeyValuePair<KeyType, ValueType>(nullableKey.value, value));
        }

        bool ICollection<KeyValuePair<YetNullable<KeyType>, ValueType>>.Remove(KeyValuePair<YetNullable<KeyType>, ValueType> item)
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

        #region IReadOnlyDictionary<YetNullable<KeyType>, ValueType>

        IEnumerable<YetNullable<KeyType>> IReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Keys {
            get {
                var nullableKeyValuePair = this.nullableKeyValuePair;

                if (nullableKeyValuePair != null) {
                    yield return nullableKeyValuePair.Value.Key;
                }

                foreach (var key in dictionary.Keys) {
                    yield return new YetNullable<KeyType>(key, false);
                }
            }
        }

        IEnumerable<ValueType> IReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Values {
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

        ValueType IReadOnlyDictionary<YetNullable<KeyType>, ValueType>.this[YetNullable<KeyType> key] {
            get {
                if (key.IsNull) {
                    if (!nullableKeyValuePair.HasValue) {
                        throw new KeyNotFoundException("The key does not exist in the collection.");
                    }

                    return nullableKeyValuePair.Value.Value;
                }

                return dictionary[key.value];
            }
        }

        #endregion

        #region IReadOnlyCollection<KeyValuePair<IYetNullable<KeyType>,ValueType>>

        IEnumerator<KeyValuePair<IYetNullable<KeyType>, ValueType>> IEnumerable<KeyValuePair<IYetNullable<KeyType>, ValueType>>.GetEnumerator() =>
            new KeyValuePairEnumeratorWithPairHavingCovariantNullableKey<KeyType, ValueType>(GetEnumerator());

        #endregion

        #region IReadOnlyDictionary<KeyType, ValueType>

        bool IReadOnlyDictionary<KeyType, ValueType>.ContainsKey(KeyType key) =>
            ContainsKey(key);

        ValueType IReadOnlyDictionary<KeyType, ValueType>.this[KeyType key] =>
            this[key];

        #endregion

        #region

        bool ICovariantReadOnlyDictionary<KeyType, ValueType>.ContainsKey(KeyType key) =>
            ContainsKey(key);

        ValueType ICovariantReadOnlyDictionary<KeyType, ValueType>.this[KeyType key] =>
            this[key];

        #endregion

        #region ICovariantReadOnlyNullableKeyDictionary<KeyType, ValueType>

        IEnumerable<KeyType> ICovariantReadOnlyNullableKeyDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        IEnumerable<ValueType> ICovariantReadOnlyNullableKeyDictionary<KeyType, ValueType>.Values => dictionary.Values;

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyNullableKeyDictionary<KeyType, ValueType>.TryGetValue(YetNullable<KeyType> key) =>
            FindValue(key);

        #region ICovariantReadOnlyDictionary<KeyType, ValueType>

        IEnumerable<KeyType> ICovariantReadOnlyDictionary<KeyType, ValueType>.Keys => dictionary.Keys;
        IEnumerable<ValueType> ICovariantReadOnlyDictionary<KeyType, ValueType>.Values => dictionary.Values;

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyDictionary<KeyType, ValueType>.TryGetValue(KeyType key) =>
            FindValue(key);

        #endregion

        #region IEnumerable<ICovariantKeyValuePair<KeyType, ValueType>>

        IEnumerator<ICovariantKeyValuePair<KeyType, ValueType>> IEnumerable<ICovariantKeyValuePair<KeyType, ValueType>>.GetEnumerator() =>
            this.AsReadOnlyDictionary().ToCovariantKeyValuePairCollection().GetEnumerator();

        #endregion

        #region ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>

        IEnumerable<YetNullable<KeyType>> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Keys => Keys;
        IEnumerable<ValueType> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Values => Values;

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.TryGetValue(YetNullable<KeyType> key) =>
            FindValue(key);

        #endregion

        #region IEnumerable<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>>

        IEnumerator<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>> IEnumerable<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>>.GetEnumerator() =>
            this.AsReadOnlyDictionaryWithNullableKeys().ToCovariantKeyValuePairCollection().GetEnumerator();

        #endregion

        #endregion

        internal static class NullableKeyDictionaryExceptionHelper
        {
            public static NotSupportedException CreateNotSupportedException() =>
                new NotSupportedException($"The {nameof(IReadOnlyNullableKeyDictionary<KeyType, ValueType>)} is read-only.");
        }
    }
}
