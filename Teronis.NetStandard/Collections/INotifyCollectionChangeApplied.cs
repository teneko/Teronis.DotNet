using Teronis.Collections.ObjectModel;

namespace Teronis.Collections
{
    public interface INotifyCollectionChangeApplied<TItem>
    {
        event CollectionChangeAppliedEventHandler<TItem> CollectionChangeApplied;
    }
}
