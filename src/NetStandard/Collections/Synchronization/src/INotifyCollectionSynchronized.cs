using System;

namespace Teronis.Collections.Synchronization
{
    public interface INotifyCollectionSynchronized<ItemType>
    {
        event EventHandler CollectionSynchronized;
    }
}
