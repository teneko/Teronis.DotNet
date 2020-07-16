using System.Collections.Generic;
using System.Threading.Tasks;
using MorseCode.ITask;

namespace Teronis.Collections.Synchronization
{
    public interface ISynchronizeCollectionAsync<in ItemType>
    {
        Task SynchronizeAsync(ITask<IEnumerable<ItemType>> Collection);
    }
}
