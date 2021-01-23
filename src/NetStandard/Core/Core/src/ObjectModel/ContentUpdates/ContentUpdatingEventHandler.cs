namespace Teronis.ObjectModel.ContentUpdates
{
    public delegate void ContentUpdatingEventHandler<ContentType>(object sender, ContentUpdatingEventArgs<ContentType> args);
}
