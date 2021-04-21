// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Generic
{
    public static class CovariantKeyValuePairExtensions
    {
        public static IReadOnlyCollection<ICovariantKeyValuePair<IYetNullable<TKey>, TValue>> AsCovariantKeyValuePairReadOnlyCollection<TKey, TValue>(this IReadOnlyCollection<CovariantKeyValuePair<YetNullable<TKey>, TValue>> collection)
        {
            var explicitAssignedCollection = (IReadOnlyCollection<ICovariantKeyValuePair<IYetNullable<TKey>, TValue>>)collection;
            return explicitAssignedCollection;
        }
    }
}
