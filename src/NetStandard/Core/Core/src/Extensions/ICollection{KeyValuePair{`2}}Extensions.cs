// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Extensions
{
    public static class ICollection_KeyValuePair_KeyType_ValueType__Extensions
    {
        public static ICollection<KeyValuePair<KeyType, ValueType>> AsCollectionWithPairs<KeyType, ValueType>(this ICollection<KeyValuePair<KeyType, ValueType>> collection) =>
            collection;
    }
}
