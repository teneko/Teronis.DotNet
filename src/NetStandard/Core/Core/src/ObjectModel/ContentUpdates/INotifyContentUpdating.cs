namespace Teronis.ObjectModel.ContentUpdates
{
    public interface INotifyContentUpdating<ContentType>
    {
        event ContentUpdatingEventHandler<ContentType>? ContentUpdating;
    }
}
