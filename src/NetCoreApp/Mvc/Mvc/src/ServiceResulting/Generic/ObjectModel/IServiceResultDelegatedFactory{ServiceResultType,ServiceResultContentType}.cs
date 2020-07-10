

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ServiceResultType, ServiceResultContentType> : IServiceResultDelegatedFactory<ServiceResultContentType>
        where ServiceResultType : IServiceResult<ServiceResultContentType>
    {
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToSuccess();
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToSuccess([AllowNull] ServiceResultContentType content);
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToFailure();
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToFailure(IServiceResult? serviceResult);
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToFailure(JsonErrors? errors);
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToFailure(JsonError? error);
        new IServiceResultPostConfiguration<ServiceResultType, ServiceResultContentType> ToFailure(string? errorMessage);
    }
}
