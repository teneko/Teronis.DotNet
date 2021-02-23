using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class CollectionSynchronizationMethod
    {
        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in sequential order
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="equalityComparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Sequential<ItemType>(IEqualityComparer<ItemType> equalityComparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>.Sequential(
                leftItem => leftItem,
                rightItem => rightItem,
                equalityComparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in sequential order
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="equalityComparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Sequential<ItemType>()
            where ItemType : notnull =>
            Sequential(EqualityComparer<ItemType>.Default);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Ascending<ItemType>(IComparer<ItemType> comparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>.Sorted(
                leftItem => leftItem,
                rightItem => rightItem,
                SortedCollectionOrder.Ascending,
                comparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Ascending<ItemType>()
            where ItemType : notnull =>
            Ascending(Comparer<ItemType>.Default);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in descended order.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Descending<ItemType>(IComparer<ItemType> comparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>.Sorted(
                leftItem => leftItem,
                rightItem => rightItem,
                SortedCollectionOrder.Descending,
                comparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in descended order.
        /// </summary>
        /// <typeparam name="ItemType">The item type.</typeparam>
        /// <param name="comparer"></param>
        /// <returns>A collection synchronization method.</returns>
        public static ICollectionSynchronizationMethod<ItemType> Descending<ItemType>()
            where ItemType : notnull =>
            Descending(Comparer<ItemType>.Default);

        public static ICollectionSynchronizationMethod<ItemType> Sorted<ItemType>(IComparer<ItemType> comparer, bool descended)
            where ItemType : notnull
        {
            if (descended) {
                return Descending(comparer);
            }

            return Ascending(comparer);
        }
    }
}
