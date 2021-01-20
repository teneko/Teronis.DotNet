using Teronis.Collections.Synchronization;

namespace Teronis.Collections.Algorithms
{
    public interface INotifyCollectionModified<SubItemType, SuperItemType>
    {
        event NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType> CollectionModified;
    }
}
