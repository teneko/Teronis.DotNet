using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class KeyValuePairEnumeratorWithPairHavingCovariantNullableKey<KeyType, ValueType> : KeyValuePairEnumeratorWithConversionBase<KeyValuePair<IStillNullable<KeyType>, ValueType>, StillNullable<KeyType>, ValueType>
        where KeyType : notnull
    {
        public KeyValuePairEnumeratorWithPairHavingCovariantNullableKey(IEnumerator<KeyValuePair<StillNullable<KeyType>, ValueType>> enumerator)
            : base(enumerator)
        { }

        protected override KeyValuePair<IStillNullable<KeyType>, ValueType> CreateCurrent(KeyValuePair<StillNullable<KeyType>, ValueType> currentPair) =>
            new KeyValuePair<IStillNullable<KeyType>, ValueType>(currentPair.Key, currentPair.Value);
    }
}
