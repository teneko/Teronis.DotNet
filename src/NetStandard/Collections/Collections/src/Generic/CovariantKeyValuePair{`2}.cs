// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Collections.Generic
{
    public readonly struct CovariantKeyValuePair<TKey, TValue> : ICovariantKeyValuePair<TKey, TValue>
    {
        public readonly TKey Key { get; }
        public readonly TValue Value { get; }

        public CovariantKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public void Deconstruct(out TKey key, out TValue value)
        {
            key = Key;
            value = Value;
        }
    }
}
