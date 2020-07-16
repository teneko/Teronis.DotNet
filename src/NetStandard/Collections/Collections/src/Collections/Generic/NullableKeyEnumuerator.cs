using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Teronis.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public class NullableKeyEnumuerator<KeyType, ValueType> : IEnumerator<KeyValuePair<NullableKey<KeyType>, ValueType>>
        where KeyType : notnull
    {
        [MaybeNull]
        public KeyValuePair<NullableKey<KeyType>, ValueType> Current { get; private set; }

        private bool movedOverNull;
        private readonly IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator;
        private readonly KeyValuePair<NullableKey<KeyType>, ValueType>? nullableKeyValuePair;

        object? IEnumerator.Current => Current;

        public NullableKeyEnumuerator(IEnumerator<KeyValuePair<KeyType, ValueType>> enumerator, KeyValuePair<NullableKey<KeyType>, ValueType>? nullableKeyValuePair)
        {
            this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            movedOverNull = nullableKeyValuePair is null;
            this.nullableKeyValuePair = nullableKeyValuePair;
        }

        public void Dispose() =>
            enumerator.Dispose();

        public bool MoveNext()
        {
            if (!movedOverNull) {
                Current = nullableKeyValuePair!.Value;
                movedOverNull = true;
                return true;
            }

            if (enumerator.MoveNext()) {
                var (key, value) = enumerator.Current;
                var nullableKey = new NullableKey<KeyType>(key, false);
                Current = new KeyValuePair<NullableKey<KeyType>, ValueType>(nullableKey, value);
                return true;
            }

            return false;
        }

        public void Reset() =>
            enumerator.Reset();
    }
}
