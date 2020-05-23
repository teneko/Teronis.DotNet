

namespace Teronis.ObjectModel.Updates
{
    public delegate void ContentUpdatedEventHandler<in ContentType>(object sender, IContentUpdatedEventArgs<ContentType> update);
}
