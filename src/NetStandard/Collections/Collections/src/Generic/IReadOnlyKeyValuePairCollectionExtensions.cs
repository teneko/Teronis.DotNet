// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public static class IReadOnlyKeyValuePairCollectionExtensions
    {
        [return: NotNullIfNotNull("readOnlyCollection")]
        public static CovariantKeyValuePairCollection<KeyType, ValueType>? ToCovariantKeyValuePairCollection<KeyType, ValueType>(this IReadOnlyCollection<KeyValuePair<KeyType, ValueType>>? readOnlyCollection)
            where KeyType : notnull =>
            readOnlyCollection is null ? null : new CovariantKeyValuePairCollection<KeyType, ValueType>(readOnlyCollection);

        [return: NotNullIfNotNull("readOnlyCollection")]
        public static CovariantKeyValuePairCollection<YetNullable<KeyType>, ValueType>? ToCovariantKeyValuePairCollectionWithNullableKeys<KeyType, ValueType>(this IReadOnlyCollection<KeyValuePair<YetNullable<KeyType>, ValueType>>? readOnlyCollection)
            where KeyType : notnull =>
            readOnlyCollection is null ? null : new CovariantKeyValuePairCollection<YetNullable<KeyType>, ValueType>(readOnlyCollection);
    }
}
