using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public struct CovariantKeyValuePair<KeyType, ValueType> : ICovariantKeyValuePair<KeyType, ValueType>
    {
        public KeyType Key { get; }

        [MaybeNull, AllowNull]
        public ValueType Value { get; }

        public CovariantKeyValuePair(KeyType key, [AllowNull] ValueType value)
        {
            Key = key;
            Value = value;
        }

        public void Deconstruct(out KeyType key, [MaybeNull] out ValueType value)
        {
            key = Key;
            value = Value;
        }
    }
}
