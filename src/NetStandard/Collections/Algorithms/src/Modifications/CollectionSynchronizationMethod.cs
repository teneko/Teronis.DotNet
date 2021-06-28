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
        public static ICollectionSynchronizationMethod<TItem> Sorted<TItem>(IComparer<TItem> comparer)
            where TItem : notnull =>
            new CollectionSynchronizationMethod<TItem>.Sorted(
                leftItem => leftItem,
                rightItem => rightItem,
                comparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<TItem> Sorted<TItem>()
            where TItem : notnull =>
            Sorted(Comparer<TItem>.Default);
    }
}
