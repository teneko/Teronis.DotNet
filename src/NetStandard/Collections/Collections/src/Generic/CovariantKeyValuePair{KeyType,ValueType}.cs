using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public struct CovariantKeyValuePair<KeyType, ValueType> : ICovariantKeyValuePair<KeyType, ValueType>
    {
        public CovariantKeyValuePair(KeyType key, [AllowNull] ValueType value)
        {
            Key = key;
            Value = value;
        }

        public KeyType Key { get; }
        [MaybeNull, AllowNull]
        public ValueType Value { get; }

        public void Deconstruct(out KeyType key, [MaybeNull] out ValueType value)
        {
            key = Key;
            value = Value;
        }
    }
}
