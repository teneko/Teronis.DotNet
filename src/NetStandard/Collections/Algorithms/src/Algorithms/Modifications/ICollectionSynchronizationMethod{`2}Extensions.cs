// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionSynchronizationMethodExtensions
    {
        public static IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications<TLeftItem, TRightItem>(
            this ICollectionSynchronizationMethod<TLeftItem, TRightItem> synchronizationMethod,
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem>? rightItems) =>
            synchronizationMethod.YieldCollectionModifications(leftItems, rightItems, CollectionModificationYieldCapabilities.All);
    }
}
