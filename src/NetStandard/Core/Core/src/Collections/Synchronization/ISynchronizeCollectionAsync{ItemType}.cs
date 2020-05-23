using System.Collections.Generic;
using System.Threading.Tasks;
using Teronis.ObjectModel.Updates;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizeCollectionAsync<in ItemType>
    {
        Task SynchronizeAsync(IContentUpdate<IEnumerable<ItemType>> Collection);
    }
}
