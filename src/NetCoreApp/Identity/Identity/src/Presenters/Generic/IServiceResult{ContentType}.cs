using System.Diagnostics.CodeAnalysis;

namespace Teronis.Identity.Presenters.Generic
{
    public interface IServiceResult<out ContentType> : IServiceResult
    {
        [MaybeNull, AllowNull]
        ContentType Content { get; }
    }
}
