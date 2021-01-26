using Teronis.Collections.Synchronization;

namespace Teronis.Collections.Algorithms
{
    public interface INotifyCollectionModification<SubItemType, SuperItemType>
    {
        event NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType> CollectionModified;
    }
}
