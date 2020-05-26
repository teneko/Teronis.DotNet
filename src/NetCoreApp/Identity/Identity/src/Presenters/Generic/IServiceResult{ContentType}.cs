

namespace Teronis.Identity.Presenters.Generic
{
    public interface IServiceResult<out ContentType> : IServiceResult
    {
        ContentType Content { get; }
    }
}
