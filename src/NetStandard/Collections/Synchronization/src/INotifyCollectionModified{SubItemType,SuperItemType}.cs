namespace Teronis.Collections.Changes
{
    public interface INotifyCollectionModified<SubItemType, SuperItemType>
    {
        event NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType> CollectionModified;
    }
}
