

namespace Teronis.Collections.Changes
{
    public interface INotifyCollectionChangeApplied<ItemType, ContentType>
    {
        event CollectionChangeAppliedEventHandler<ItemType, ContentType> CollectionChangeApplied;
    }
}
