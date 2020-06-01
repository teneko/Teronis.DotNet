

using System.Diagnostics.CodeAnalysis;
using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Presenters.Generic.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ServiceResultType, ServiceResultContentType> : IServiceResultDelegatedFactory<ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToSuccess();
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToSuccess([AllowNull]ServiceResultContentType content);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailure();
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailure(IServiceResult? serviceResult);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailure(JsonErrors? errors);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailure(JsonError? error);
        new IServiceResultFactory<ServiceResultType, ServiceResultContentType> ToFailure(string? errorMessage);
    }
}
