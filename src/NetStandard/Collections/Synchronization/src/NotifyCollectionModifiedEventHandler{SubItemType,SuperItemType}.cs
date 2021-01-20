namespace Teronis.Collections.Synchronization
{
    public delegate void NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType>(object sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args);
}
