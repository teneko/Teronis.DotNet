using Teronis.Data;

namespace Teronis.ObjectModel.Updates
{

    public interface IApplyContentUpdate<ContentType> : IWorking, IApplyContentUpdateBy<ContentType>
    {
        event ContentUpdatingEventHandler<ContentType> ContainerUpdating;
        event ContentUpdatedEventHandler<ContentType> ContainerUpdated;

        bool IsContentUpdateAppliable(IContentUpdate<ContentType> update);
    }
}
