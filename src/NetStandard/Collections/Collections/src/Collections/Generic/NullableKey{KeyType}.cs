using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public struct NullableKey<KeyType> : IEquatable<NullableKey<KeyType>>, IEquatable<KeyType>, INullableKey<KeyType>
        where KeyType : notnull
    {
        public static NullableKey<KeyType> Null = new NullableKey<KeyType>(default, true);

        [AllowNull, MaybeNull]
        public readonly KeyType Key { get; }
        public readonly bool IsNull => !isNotNull;

        private readonly bool isNotNull;

        internal NullableKey([AllowNull] KeyType key, bool isNull)
        {
            Key = key;
            isNotNull = !isNull;
        }

        public NullableKey([AllowNull] KeyType key)
            : this(key, key is null) { }

        public bool Equals([AllowNull] KeyType other)
        {
            if (other is null || IsNull) {
                return false;
            }
            
            return Key!.Equals(other);
        }

        public bool Equals([AllowNull] NullableKey<KeyType> other) =>
            Equals(other.Key);

        public override bool Equals(object? obj)
        {
            if (obj is NullableKey<KeyType> nullableKey) {
                return Equals(nullableKey);
            }

            if (obj is KeyType key) {
                return Equals(key);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (IsNull) {
                return 0;
            }

            return Key!.GetHashCode();
        }

        public override string? ToString() =>
            Key?.ToString();

        public static implicit operator NullableKey<KeyType>(KeyType key) =>
            new NullableKey<KeyType>(key);

        public static implicit operator KeyType(NullableKey<KeyType> key) =>
            key.Key!;
    }
}
