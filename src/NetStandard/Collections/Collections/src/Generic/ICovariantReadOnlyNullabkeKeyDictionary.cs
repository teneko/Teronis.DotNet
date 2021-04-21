// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public interface ICovariantReadOnlyNullableKeyDictionary<TKey, out TValue> :
        ICovariantReadOnlyDictionary<TKey, TValue>, ICovariantReadOnlyDictionary<YetNullable<TKey>, TValue>,
        IEnumerable
        where TKey : notnull
    {
        new TValue this[YetNullable<TKey> key] { get; }

        new IEnumerable<TKey> Keys { get; }
        new IEnumerable<TValue> Values { get; }
        new int Count { get; }

        new bool ContainsKey(YetNullable<TKey> key);
        new ICovariantTuple<bool, TValue> TryGetValue(YetNullable<TKey> key);
    }
}
