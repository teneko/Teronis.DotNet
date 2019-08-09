

namespace Teronis.Data
{

    public interface IUpdatableContent<in ContravariantContentType, out CovariantContentType> : IContentUpdateSequenceStatus, IUpdatableContentBy<ContravariantContentType>
        //where ContravariantContentType : CovariantContentType
    {
        event UpdatingEventHandler<CovariantContentType> ContainerUpdating;
        event UpdatedEventHandler<CovariantContentType> ContainerUpdated;

        bool IsContentUpdatable(IUpdate<ContravariantContentType> update);
    }

    public interface IUpdatableContent<ContentType> : IUpdatableContent<ContentType, ContentType>
    { }
}
