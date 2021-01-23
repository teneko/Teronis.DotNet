namespace Teronis.ObjectModel.ContentUpdates
{
    public delegate void ContentUpdatedEventHandler<ContentType>(object sender, ContentUpdatedEventArgs<ContentType> update);
}
