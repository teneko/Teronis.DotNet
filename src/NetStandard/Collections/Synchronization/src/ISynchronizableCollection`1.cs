using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizableCollection<in ItemType>
    {
        void SynchronizeCollection(IEnumerable<ItemType>? enumerable);
    }
}
