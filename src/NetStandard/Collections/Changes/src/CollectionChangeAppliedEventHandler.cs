

namespace Teronis.Collections.Changes
{
    public delegate void CollectionChangeAppliedEventHandler<ItemType, ContentType>(object sender, CollectionChangeAppliedEventArgs<ItemType, ContentType> args);
}
