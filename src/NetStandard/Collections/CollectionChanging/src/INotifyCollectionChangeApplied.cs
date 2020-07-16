

namespace Teronis.Collections.CollectionChanging
{
    public interface INotifyCollectionChangeApplied<ItemType, ContentType>
    {
        event CollectionChangeAppliedEventHandler<ItemType, ContentType> CollectionChangeApplied;
    }
}
