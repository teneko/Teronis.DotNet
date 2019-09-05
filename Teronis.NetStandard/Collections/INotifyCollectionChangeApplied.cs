using Teronis.Collections.ObjectModel;

namespace Teronis.Collections
{
    public interface INotifyCollectionChangeApplied<ItemType, ContentType>
    {
        event CollectionChangeAppliedEventHandler<ItemType, ContentType> CollectionChangeApplied;
    }
}
