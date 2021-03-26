// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface IReadOnlyNullableKeyDictionary<KeyType, ValueType> : IReadOnlyDictionary<KeyType, ValueType>, IReadOnlyDictionary<YetNullable<KeyType>, ValueType>,
        IReadOnlyCollection<KeyValuePair<IYetNullable<KeyType>, ValueType>>, ICovariantReadOnlyNullableKeyDictionary<KeyType, ValueType>
        where KeyType : notnull
    {
        new ValueType this[YetNullable<KeyType> key] { get; }

        new int Count { get; }
        new ICollection<YetNullable<KeyType>> Keys { get; }
        new ICollection<ValueType> Values { get; }

        new bool ContainsKey(YetNullable<KeyType> key);
        new bool TryGetValue(YetNullable<KeyType> key, [MaybeNullWhen(false)] out ValueType value);

        new IEnumerator<KeyValuePair<YetNullable<KeyType>, ValueType>> GetEnumerator();
    }
}
