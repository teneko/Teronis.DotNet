// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionSynchronizationMethod<TLeftItem, TRightItem>
    {
        IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem>? rightItems,
            CollectionModificationYieldCapabilities yieldCapabilities);
    }
}
