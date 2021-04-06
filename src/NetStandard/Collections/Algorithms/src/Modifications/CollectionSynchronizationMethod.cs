// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class CollectionSynchronizationMethod
    {
        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in sequential order
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="equalityComparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Sequential<TItem>(IEqualityComparer<TItem> equalityComparer)
            where TItem : notnull =>
            new CollectionSynchronizationMethod<TItem>.Sequential(
                leftItem => leftItem,
                rightItem => rightItem,
                equalityComparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in sequential order
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Sequential<TItem>()
            where TItem : notnull =>
            Sequential(EqualityComparer<TItem>.Default);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Ascending<TItem>(IComparer<TItem> comparer)
            where TItem : notnull =>
            new CollectionSynchronizationMethod<TItem>.Sorted(
                leftItem => leftItem,
                rightItem => rightItem,
                SortedCollectionOrder.Ascending,
                comparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Ascending<TItem>()
            where TItem : notnull =>
            Ascending(Comparer<TItem>.Default);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in descended order.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Descending<TItem>(IComparer<TItem> comparer)
            where TItem : notnull =>
            new CollectionSynchronizationMethod<TItem>.Sorted(
                leftItem => leftItem,
                rightItem => rightItem,
                SortedCollectionOrder.Descending,
                comparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in descended order.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Descending<TItem>()
            where TItem : notnull =>
            Descending(Comparer<TItem>.Default);

        public static ICollectionSynchronizationMethod<TItem> Sorted<TItem>(IComparer<TItem> comparer, bool descended)
            where TItem : notnull
        {
            if (descended) {
                return Descending(comparer);
            }

            return Ascending(comparer);
        }
    }
}
