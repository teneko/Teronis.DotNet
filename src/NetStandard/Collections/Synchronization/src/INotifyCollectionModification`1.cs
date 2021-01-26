namespace Teronis.Collections.Synchronization
{
    public interface INotifyCollectionModification<ItemType>
    {
        event NotifyNotifyCollectionModifiedEventHandler<ItemType> CollectionModified;
    }
}
