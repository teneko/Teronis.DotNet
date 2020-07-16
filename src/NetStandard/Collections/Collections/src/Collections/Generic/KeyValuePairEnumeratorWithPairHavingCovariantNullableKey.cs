using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class KeyValuePairEnumeratorWithPairHavingCovariantNullableKey<KeyType, ValueType> : KeyValuePairEnumeratorWithConversionBase<KeyValuePair<INullableKey<KeyType>, ValueType>, NullableKey<KeyType>, ValueType>
        where KeyType : notnull
    {
        public KeyValuePairEnumeratorWithPairHavingCovariantNullableKey(IEnumerator<KeyValuePair<NullableKey<KeyType>, ValueType>> enumerator)
            : base(enumerator)
        { }

        protected override KeyValuePair<INullableKey<KeyType>, ValueType> CreateCurrent(KeyValuePair<NullableKey<KeyType>, ValueType> currentPair) =>
            new KeyValuePair<INullableKey<KeyType>, ValueType>(currentPair.Key, currentPair.Value);
    }
}
