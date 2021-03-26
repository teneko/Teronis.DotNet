// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Data
{
    public class MutableValue<T> : IEquatable<T> where T : IEquatable<T>
    {
        public event EventHandler<T>? ValueMutated;

        [MaybeNull, AllowNull]
        public T Value { get; private set; }

        public MutableValue([AllowNull] T value)
        {
            Value = value;
        }

        public void MutateValue(T mutation)
        {
            Value = mutation;
            ValueMutated?.Invoke(this, Value);
        }

        public override bool Equals(object? obj)
        {
            if (Value is null) {
                return false;
            }

            if (obj is MutableValue<T> mutation) {
                if (mutation is null) {
                    return false;
                }

                return Value.Equals(mutation.Value!);
            }

            if (obj is T val) {
                return Value.Equals(val);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Value is null) {
                return 0;
            }

            return Value.GetHashCode();
        }

        public bool Equals([AllowNull] T other)
        {
            if (Value is null) {
                return false;
            }

            return Value.Equals(other!);
        }

        public override string? ToString()
        {
            if (Value is null) {
                return null;
            }

            return Value.ToString();
        }
    }
}
