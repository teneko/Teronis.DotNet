using MorseCode.ITask;

namespace Teronis.Data
{
    public interface IAsyncContentUpdate<out ContentType> : IContentUpdate
    {
        new ITask<ContentType> Content { get; }
    }
}
