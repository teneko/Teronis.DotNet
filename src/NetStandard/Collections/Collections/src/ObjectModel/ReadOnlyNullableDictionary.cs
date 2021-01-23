using System.Collections;
using System.Collections.Generic;
using Teronis.Collections.Generic;

namespace Teronis.Collections.ObjectModel
{
    public class ReadOnlyNullableDictionary<KeyType, ValueType> : IReadOnlyNullableKeyDictionary<KeyType, ValueType>
        where KeyType : notnull
    {

        public ValueType this[YetNullable<KeyType> key] =>
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary)[key];

        public int Count =>
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).Count;

        public ICollection<YetNullable<KeyType>> Keys =>
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).Keys;

        public ICollection<ValueType> Values =>
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).Values;

        private INullableKeyDictionary<KeyType, ValueType> dictionary;

        public ReadOnlyNullableDictionary(INullableKeyDictionary<KeyType, ValueType> dictionary) => 
            this.dictionary = dictionary;

        public bool ContainsKey(YetNullable<KeyType> key) => 
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).ContainsKey(key);

        public bool TryGetValue(YetNullable<KeyType> key, out ValueType value) => 
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).TryGetValue(key, out value);

        public bool ContainsKey(KeyType key) => 
            ((IReadOnlyDictionary<KeyType, ValueType>)dictionary).ContainsKey(key);

        public bool TryGetValue(KeyType key, out ValueType value) => 
            ((IReadOnlyDictionary<KeyType, ValueType>)dictionary).TryGetValue(key, out value);

        public ValueType this[KeyType key] => 
            ((IReadOnlyDictionary<KeyType, ValueType>)dictionary)[key];

        public IEnumerator<KeyValuePair<YetNullable<KeyType>, ValueType>> GetEnumerator() =>
            ((IReadOnlyNullableKeyDictionary<KeyType, ValueType>)dictionary).GetEnumerator();

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyDictionary<KeyType, ValueType>.TryGetValue(KeyType key) => ((ICovariantReadOnlyDictionary<KeyType, ValueType>)dictionary).TryGetValue(key);

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>.TryGetValue(YetNullable<KeyType> key) => ((ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>)dictionary).TryGetValue(key);

        ICovariantTuple<bool, ValueType> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.TryGetValue(YetNullable<KeyType> key) => ((ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>)dictionary).TryGetValue(key);


        IEnumerable<KeyType> IReadOnlyDictionary<KeyType, ValueType>.Keys => ((IReadOnlyDictionary<KeyType, ValueType>)dictionary).Keys;

        IEnumerable<ValueType> IReadOnlyDictionary<KeyType, ValueType>.Values => ((IReadOnlyDictionary<KeyType, ValueType>)dictionary).Values;

        IEnumerator<KeyValuePair<KeyType, ValueType>> IEnumerable<KeyValuePair<KeyType, ValueType>>.GetEnumerator() => ((IEnumerable<KeyValuePair<KeyType, ValueType>>)dictionary).GetEnumerator();

        IEnumerable<YetNullable<KeyType>> IReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Keys => ((IReadOnlyDictionary<YetNullable<KeyType>, ValueType>)dictionary).Keys;

        IEnumerable<ValueType> IReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Values => ((IReadOnlyDictionary<YetNullable<KeyType>, ValueType>)dictionary).Values;

        IEnumerator<KeyValuePair<IYetNullable<KeyType>, ValueType>> IEnumerable<KeyValuePair<IYetNullable<KeyType>, ValueType>>.GetEnumerator() => ((IEnumerable<KeyValuePair<IYetNullable<KeyType>, ValueType>>)dictionary).GetEnumerator();

        IEnumerable<KeyType> ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>.Keys => ((ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>)dictionary).Keys;

        IEnumerable<ValueType> ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>.Values => ((ICovariantReadOnlyNullabkeKeyDictionary<KeyType, ValueType>)dictionary).Values;


        IEnumerable<KeyType> ICovariantReadOnlyDictionary<KeyType, ValueType>.Keys => ((ICovariantReadOnlyDictionary<KeyType, ValueType>)dictionary).Keys;

        IEnumerable<ValueType> ICovariantReadOnlyDictionary<KeyType, ValueType>.Values => ((ICovariantReadOnlyDictionary<KeyType, ValueType>)dictionary).Values;

        IEnumerator<ICovariantKeyValuePair<KeyType, ValueType>> IEnumerable<ICovariantKeyValuePair<KeyType, ValueType>>.GetEnumerator() => ((IEnumerable<ICovariantKeyValuePair<KeyType, ValueType>>)dictionary).GetEnumerator();

        IEnumerable<YetNullable<KeyType>> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Keys => ((ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>)dictionary).Keys;

        IEnumerable<ValueType> ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>.Values => ((ICovariantReadOnlyDictionary<YetNullable<KeyType>, ValueType>)dictionary).Values;

        IEnumerator<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>> IEnumerable<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>>.GetEnumerator() => ((IEnumerable<ICovariantKeyValuePair<YetNullable<KeyType>, ValueType>>)dictionary).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();
    }
}
