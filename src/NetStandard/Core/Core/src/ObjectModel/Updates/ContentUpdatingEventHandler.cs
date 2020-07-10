namespace Teronis.ObjectModel.Updates
{
    public delegate void ContentUpdatingEventHandler<in ContentType>(object sender, IContentUpdatingEventArgs<ContentType> args);
}
