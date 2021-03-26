// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePairExtensions
    {
        public static List<CovariantKeyValuePair<KeyType, ValueType>> ToList<KeyType, ValueType>(this CovariantKeyValuePair<KeyType, ValueType> pair) =>
            new List<CovariantKeyValuePair<KeyType, ValueType>>() { { pair } };

        public static IReadOnlyCollection<ICovariantKeyValuePair<IYetNullable<KeyType>, ValueType>> AsCovariantList<KeyType, ValueType>(this IReadOnlyCollection<CovariantKeyValuePair<YetNullable<KeyType>, ValueType>> collection) {
            var explicitAssignedCollection = (IReadOnlyCollection<ICovariantKeyValuePair<IYetNullable<KeyType>, ValueType>>)collection;
            return explicitAssignedCollection;
        }
    }
}
