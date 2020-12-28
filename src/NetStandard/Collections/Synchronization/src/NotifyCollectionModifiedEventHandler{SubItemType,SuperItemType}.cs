using Teronis.Collections.Changes;

namespace Teronis.Collections.Synchronization
{
    public delegate void CollectionModifiedEventHandler<SubItemType, SuperItemType>(object sender, CollectionModifiedEventArgs<SubItemType, SuperItemType> args);
}
