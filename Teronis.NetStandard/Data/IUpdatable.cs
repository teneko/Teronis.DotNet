

using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IUpdatable<T> : IUpdateSequenceStatus
    {
        event UpdatingEventHandler<T> Updating;
        event UpdatedEventHandler<T> Updated;
        
        bool IsUpdatable(Update<T> update);
        void UpdateBy(Update<T> update);
        Task UpdateByAsync(Update<T> update);
    }
}
