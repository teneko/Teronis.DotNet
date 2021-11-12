// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionSynchronizationMethod<TLeftItem, TRightItem>
    {
        /// <summary>
        /// Yields the collection modifications to transform left items to right items.
        /// If <paramref name="disableSortWhenComparerUsed"/> is not disabled, then right items
        /// are computed immediatelly. If  <paramref name="disableSortWhenComparerUsed"/> is disabled,
        /// it expects <paramref name="rightItems"/> to be already sorted, if <see cref="IComparer{T}"/>
        /// is used internally.
        /// </summary>
        /// <param name="leftItems"></param>
        /// <param name="rightItems"></param>
        /// <param name="yieldCapabilities"></param>
        /// <param name="disableSortWhenComparerUsed">
        /// Pre-sorts <paramref name="rightItems"/> if <see cref="IComparer{T}"/> is used internally.
        /// If <see langword="true"/> you disable the pre-sort of <paramref name="rightItems"/>.
        /// </param>
        /// <returns></returns>
        IEnumerable<CollectionModification<TRightItem, TLeftItem>> YieldCollectionModifications(
            IEnumerable<TLeftItem> leftItems,
            IEnumerable<TRightItem>? rightItems,
            CollectionModificationYieldCapabilities yieldCapabilities,
            bool disableSortWhenComparerUsed = false);
    }
}
