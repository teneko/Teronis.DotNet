using System;

namespace Teronis.Collections.Synchronization
{
    public interface INotifyCollectionSynchronizing<ItemType>
    {
        event EventHandler CollectionSynchronizing;
    }
}
