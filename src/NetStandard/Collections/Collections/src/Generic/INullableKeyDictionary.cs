// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface INullableKeyDictionary<KeyType, ValueType> : IDictionary<KeyType, ValueType>, IDictionary<YetNullable<KeyType>, ValueType>
        where KeyType : notnull
    {
        new ValueType this[YetNullable<KeyType> key] { get; }

        new int Count { get; }
        new bool IsReadOnly { get; }
        new ICollection<YetNullable<KeyType>> Keys { get; }
        new ICollection<ValueType> Values { get; }

        new bool ContainsKey(YetNullable<KeyType> key);
        new bool TryGetValue(YetNullable<KeyType> key, [MaybeNullWhen(false)] out ValueType value);

        new void Clear();

        /// <summary>
        /// Adds an element associated with the nullable key.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Null key exists already.</exception>
        /// <exception cref="NotSupportedException"><see cref="INullableKeyDictionary{KeyType, ValueType}"/> is read-only.</exception>
        void Add(ValueType value);

        /// <summary>
        /// Removes the element associated with the nullable key.
        /// </summary>
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>
        /// <exception cref="NotSupportedException"><see cref="INullableKeyDictionary{KeyType, ValueType}"/> is read-only.</exception>
        bool Remove();

        new IEnumerator<KeyValuePair<YetNullable<KeyType>, ValueType>> GetEnumerator();
    }
}
