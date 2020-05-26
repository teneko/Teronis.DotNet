

namespace Teronis.Identity.Presenters.Generic
{
    public interface IServiceResultFactory<ServiceResultType, ServiceResultContentType> : IServiceResultFactory
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        ServiceResultType AsServiceResult();
    }
}
