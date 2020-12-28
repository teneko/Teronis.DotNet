namespace Teronis.Collections.Synchronization
{
    public interface INotifyCollectionModified<ItemType>
    {
        event NotifyNotifyCollectionModifiedEventHandler<ItemType> CollectionModified;
    }
}
