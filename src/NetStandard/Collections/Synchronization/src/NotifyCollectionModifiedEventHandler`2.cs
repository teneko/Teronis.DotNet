namespace Teronis.Collections.Synchronization
{
    public delegate void NotifyCollectionModifiedEventHandler<SuperItemType, SubItemType>(object sender, CollectionModifiedEventArgs<SuperItemType, SubItemType> args);
}
