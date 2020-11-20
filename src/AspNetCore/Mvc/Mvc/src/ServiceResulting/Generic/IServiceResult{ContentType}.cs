using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public interface IServiceResult<out ContentType> : IServiceResult
    {
        [MaybeNull]
        new ContentType Content { get; }
    }
}
