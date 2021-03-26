// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



using System.Diagnostics.CodeAnalysis;

namespace Teronis.Data
{
    public class WrappedValue<T>
    {
        [AllowNull, MaybeNull]
        public virtual T Value { get; set; }

        public WrappedValue([AllowNull] T value) => Value = value;

        [return: MaybeNull]
        public virtual T GetValue() => Value;

        public static implicit operator WrappedValue<T>(T value) => new WrappedValue<T>(value);
    }
}
