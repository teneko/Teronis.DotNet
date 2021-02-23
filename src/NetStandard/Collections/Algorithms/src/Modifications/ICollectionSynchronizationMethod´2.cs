using System.Collections.Generic;

namespace Teronis.Collections.Algorithms.Modifications
{
    public interface ICollectionSynchronizationMethod<LeftItemType, RightItemType>
    {
        IEnumerable<CollectionModification<RightItemType, LeftItemType>> YieldCollectionModifications(
            IEnumerable<LeftItemType> leftItems, 
            IEnumerable<RightItemType>? rightItems, 
            CollectionModificationsYieldCapabilities yieldCapabilities);
    }
}
