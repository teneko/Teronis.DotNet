using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public struct StillNullable<T> : IEquatable<StillNullable<T>>, IStillNullable<T>
        where T : notnull
    {
        public static StillNullable<T> Null = new StillNullable<T>(default, true);

        [MaybeNull]
        public readonly T Value => value;
        public readonly bool HasValue => isNotNull;

        internal readonly T value;

        private readonly bool isNotNull;

        internal StillNullable([AllowNull] T value, bool isNull)
        {
            this.value = value!;
            isNotNull = !isNull;
        }

        public StillNullable([AllowNull] T key)
            : this(key, key is null) { }

        public override bool Equals(object? other)
        {
            if (!HasValue) {
                return other == null;
            }

            if (other == null) {
                return false;
            }

            return Value!.Equals(other);
        }

        public bool Equals(StillNullable<T> other) =>
            StillNullable.Equals(this, other);

        public override int GetHashCode()
        {
            if (HasValue) {
                return Value!.GetHashCode();
            }

            return 0;
        }

        public override string? ToString() =>
            Value?.ToString() ?? "";

        public static implicit operator StillNullable<T>(T key) =>
            new StillNullable<T>(key);

        //public static implicit operator KeyType(StillNullable<KeyType> key) =>
        //    key.Key!;
    }
}
