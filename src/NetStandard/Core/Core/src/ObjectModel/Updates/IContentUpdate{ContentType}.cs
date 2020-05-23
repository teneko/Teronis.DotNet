using MorseCode.ITask;

namespace Teronis.ObjectModel.Updates
{
    public interface IContentUpdate<out ContentType> : IContentUpdate
    {
        new ITask<ContentType> ContentTask { get; }
    }
}
