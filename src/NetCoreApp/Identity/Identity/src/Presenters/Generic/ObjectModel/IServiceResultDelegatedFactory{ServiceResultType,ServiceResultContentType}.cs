

using Teronis.Identity.Presenters.Generic;
using Teronis.Identity.Presenters.ObjectModel;

namespace Teronis.Identity.Presenters.Generic.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ServiceResultType, ServiceResultContentType> : IServiceResultDelegatedFactory<ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToSucceeded();
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToSucceededWithContent(ServiceResultContentType content);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailed();
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailed(IServiceResult serviceResult);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailedWithJsonError(JsonError error);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailedWithErrorMessage(string errorMessage);
    }
}
