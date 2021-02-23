using Teronis.Collections.Synchronization;

namespace Teronis.Collections.Algorithms
{
    public interface INotifyCollectionModification<SuperItemType, SubItemType>
    {
        event NotifyCollectionModifiedEventHandler<SuperItemType, SubItemType> CollectionModified;
    }
}
