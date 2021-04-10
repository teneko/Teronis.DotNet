// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public struct Valuable<T> : IValuable<T>, IYetNullable<T>
    {
        [MaybeNull]
        public readonly T Value =>
            value;

        public readonly bool HasValue =>
            hasValue;

        public readonly bool HasNoValue =>
            !hasValue;

        [MemberNotNullWhen(
            returnValue: false,
            nameof(value),
            nameof(Value))]
        public bool IsNull =>
            value is null;

        [MemberNotNullWhen(
            returnValue: true,
            nameof(value),
            nameof(Value))]
        public bool IsNotNull =>
            !(value is null);

        [MaybeNull, AllowNull]
        internal readonly T value;

        private readonly bool hasValue;

        internal Valuable([AllowNull] T value, bool hasValue)
        {
            this.value = value;
            this.hasValue = hasValue;
        }

        public Valuable([AllowNull] T key)
            : this(key, hasValue: true) { }

        public T GetValueOrDefault() =>
            value;

        public T GetValueOrDefault(T defaultValue) =>
            HasValue ? value : defaultValue;

        public override bool Equals(object? other)
        {
            if (HasNoValue) {
                return other == null;
            }

            if (value is null && other == null) {
                return true;
            }

            if (value is null || other == null) {
                return false;
            }

            return value.Equals(other);
        }

        public override int GetHashCode() =>
            value?.GetHashCode() ?? 0;

        public override string? ToString() =>
            value?.ToString() ?? "";

        public static implicit operator Valuable<T>([AllowNull] T key) =>
            new Valuable<T>(key);

        [return: MaybeNull]
        public static explicit operator T(Valuable<T> key) =>
            key.value;
    }
}
