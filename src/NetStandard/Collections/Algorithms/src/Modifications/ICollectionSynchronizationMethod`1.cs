using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionSynchronizationMethod<ItemType>
    {
        IEnumerable<CollectionModification<ItemType, ItemType>> YieldCollectionModifications(IEnumerable<ItemType> leftItems, IEnumerable<ItemType>? rightItems, CollectionModificationsYieldCapabilities yieldCapabilities);
    }
}
