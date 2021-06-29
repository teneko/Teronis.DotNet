// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionSynchronizationMethodExtensions
    {
        public static IEnumerable<CollectionModification<TItem, TItem>> YieldCollectionModifications<TItem>(
            this ICollectionSynchronizationMethod<TItem> synchronizationMethod,
            IEnumerable<TItem> leftItems,
            IEnumerable<TItem>? rightItems)
            where TItem : notnull =>
            synchronizationMethod.YieldCollectionModifications(leftItems, rightItems, CollectionModificationYieldCapabilities.All);
    }
}
