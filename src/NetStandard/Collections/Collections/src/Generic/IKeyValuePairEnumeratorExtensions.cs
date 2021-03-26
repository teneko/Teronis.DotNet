// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public static class IKeyValuePairEnumeratorExtensions
    {
        public static IEnumerator<ICovariantKeyValuePair<KeyType?, ValueType>> GetEnumeratorWithCovariantPairsHavingNullableKey<KeyType, ValueType>(this IEnumerable<KeyValuePair<KeyType, ValueType>> enumerable)
           where KeyType : struct =>
           new KeyValuePairEnumeratorWithPairAsCovariantHavingKeyAsNullable<KeyType, ValueType>(enumerable.GetEnumerator());
    }
}
