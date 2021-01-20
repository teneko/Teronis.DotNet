using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class CollectionSynchronizationMethod
    {
        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in sequential order
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        public static CollectionSynchronizationMethod<ItemType> Sequential<ItemType>(IEqualityComparer<ItemType> equalityComparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>(equalityComparer);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in ascended order
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="comparer"></param>
        /// <returns>Method to be used to synchronize</returns>
        public static CollectionSynchronizationMethod<ItemType> Ascending<ItemType>(IComparer<ItemType> comparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>(comparer, CollectionSequenceType.Ascending);

        /// <summary>
        /// Creates a method for creating modifications that can transform one
        /// collection into another collection that is in descended order.
        /// </summary>
        /// <typeparam name="ItemType"></typeparam>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static CollectionSynchronizationMethod<ItemType> Descending<ItemType>(IComparer<ItemType> comparer)
            where ItemType : notnull =>
            new CollectionSynchronizationMethod<ItemType>(comparer, CollectionSequenceType.Descending);
    }
}
