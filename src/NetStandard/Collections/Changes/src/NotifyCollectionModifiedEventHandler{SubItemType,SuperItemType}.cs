namespace Teronis.Collections.Changes
{
    public delegate void NotifyCollectionModifiedEventHandler<SubItemType, SuperItemType>(object sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args);
}
