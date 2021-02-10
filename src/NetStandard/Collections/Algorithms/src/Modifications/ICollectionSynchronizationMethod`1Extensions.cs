using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public static class ICollectionSynchronizationMethodExtensions
    {
        public static IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications<ItemType>(
            this ICollectionSynchronizationMethod<ItemType> synchronizationMethod,
            IEnumerable<ItemType> leftItems,
            IEnumerable<ItemType>? rightItems)
            where ItemType : notnull =>
            synchronizationMethod.YieldCollectionModifications(leftItems, rightItems, CollectionModificationsYieldCapabilities.All);
    }
}
