using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizeCollection<in ItemType>
    {
        void SynchronizeCollection(IEnumerable<ItemType>? Collection);
    }
}
