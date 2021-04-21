// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public struct YetNullable<T> : IYetNullable<T>
    {
        public readonly static YetNullable<T> Null = new YetNullable<T>(default, isNull: true);

        [MaybeNull]
        public readonly T Value =>
            value;

        [MemberNotNullWhen(
            returnValue: false,
            nameof(value),
            nameof(Value))]
        public readonly bool IsNull =>
            !isNotNull;

        [MemberNotNullWhen(
            returnValue: true,
            nameof(value),
            nameof(Value))]
        public readonly bool IsNotNull =>
            isNotNull;

        [MaybeNull, AllowNull]
        internal readonly T value;

        private readonly bool isNotNull;

        internal YetNullable([AllowNull] T value, bool isNull)
        {
            this.value = value;
            isNotNull = !isNull;
        }

        public YetNullable([AllowNull] T value)
            : this(value, isNull: value is null) { }

        public override bool Equals(object? other)
        {
            if (IsNull) {
                return other == null;
            }

            if (other == null) {
                return false;
            }

            return value.Equals(other);
        }

        public override int GetHashCode()
        {
            if (IsNotNull) {
                return value!.GetHashCode();
            }

            return 0;
        }

        public override string? ToString() =>
            value?.ToString() ?? "";

        public static implicit operator YetNullable<T>([AllowNull] T key) =>
            new YetNullable<T>(key);

        [return: MaybeNull]
        public static explicit operator T(YetNullable<T> key) =>
            key.value;
    }
}
