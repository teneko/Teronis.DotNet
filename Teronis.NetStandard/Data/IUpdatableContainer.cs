

using System.Threading.Tasks;

namespace Teronis.Data
{
    public interface IUpdatableContainer<ContentType> : IContainerUpdateSequenceStatus
    {
        event UpdatingEventHandler<ContentType> ContainerUpdating;
        event UpdatedEventHandler<ContentType> ContainerUpdated;
        
        bool IsContainerUpdatable(Update<ContentType> update);
        void UpdateContainerBy(Update<ContentType> update);
        Task UpdateContainerByAsync(Update<ContentType> update);
        Task UpdateContainerByAsync(Task<Update<ContentType>> updateTask);
    }
}
