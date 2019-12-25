

namespace Teronis.Data
{

    public interface IUpdatableContent<ContentType> : IWorking, IUpdatableContentBy<ContentType>
    {
        event ContentUpdatingEventHandler<ContentType> ContainerUpdating;
        event ContentUpdatedEventHandler<ContentType> ContainerUpdated;

        bool IsContentUpdatable(IContentUpdate<ContentType> update);
    }
}
