namespace Teronis.ObjectModel.ContentUpdates
{
    public interface INotifyContentUpdated<ContentType>
    {
        event ContentUpdatedEventHandler<ContentType>? ContentUpdated;
    }
}
