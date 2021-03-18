using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public struct YetNullable<T> : IEquatable<YetNullable<T>>, IYetNullable<T>
    {
        public static YetNullable<T> Null = new YetNullable<T>(default, true);

        [MaybeNull]
        public readonly T Value =>
            value;

        public readonly bool IsNull =>
            !isNotNull;

        public readonly bool IsNotNull =>
            isNotNull;

        internal readonly T value;

        private readonly bool isNotNull;

        internal YetNullable([AllowNull] T value, bool isNull)
        {
            this.value = value!;
            isNotNull = !isNull;
        }

        public YetNullable([AllowNull] T key)
            : this(key, key is null) { }

        public override bool Equals(object? other)
        {
            if (IsNull) {
                return other == null;
            }

            if (other == null) {
                return false;
            }

            return Value!.Equals(other);
        }

        public bool Equals(YetNullable<T> other) =>
            YetNullable.Equals(this, other);

        public override int GetHashCode()
        {
            if (IsNotNull) {
                return Value!.GetHashCode();
            }

            return 0;
        }

        public override string? ToString() =>
            Value?.ToString() ?? "";

        public static implicit operator YetNullable<T>([AllowNull] T key) =>
            new YetNullable<T>(key);

        public static implicit operator T(YetNullable<T> key) =>
            key.Value!;
    }
}
