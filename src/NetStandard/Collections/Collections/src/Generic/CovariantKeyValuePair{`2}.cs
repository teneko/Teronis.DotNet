// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public struct CovariantKeyValuePair<KeyType, ValueType> : ICovariantKeyValuePair<KeyType, ValueType>
    {
        public KeyType Key { get; }
        public ValueType Value { get; }

        public CovariantKeyValuePair(KeyType key, ValueType value)
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
