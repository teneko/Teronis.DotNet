using Teronis.Collections.Algorithms.Modifications;

namespace Teronis.Collections.Synchronization
{
    internal interface ICollectionSynchronizationContext<ItemType>
    {
        void BeginCollectionSynchronization();
        void GoThroughModification(ICollectionModification<ItemType, ItemType> superItemModification);
        void EndCollectionSynchronization();
    }
}
