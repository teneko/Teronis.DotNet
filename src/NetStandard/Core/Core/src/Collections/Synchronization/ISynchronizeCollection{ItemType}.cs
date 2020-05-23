using System.Collections.Generic;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizeCollection<in ItemType>
    {
        void Synchronize(IEnumerable<ItemType> Collection);
    }
}
