

namespace Teronis.Data
{
    public delegate void ContentUpdatedEventHandler<in ContentType>(object sender, IContentUpdatedEventArgs<ContentType> update);
}
