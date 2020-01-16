using System.Collections.Generic;
using System.Threading.Tasks;
using MorseCode.ITask;

namespace Teronis.Collections.Generic
{
    public interface ICanSynchronizeCollection<in ItemType>
    {
        void Synchronize(IEnumerable<ItemType> collection);
        Task SynchronizeAsync(IEnumerable<ItemType> collection);
        Task SynchronizeAsync(ITask<IEnumerable<ItemType>> collection);
    }
}
