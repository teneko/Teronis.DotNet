

namespace Teronis.ObjectModel.Updates
{
    public interface IContentUpdatedEventArgs<out ContentType>
    {
        IContentUpdate<ContentType> Update { get; }
    }
}
