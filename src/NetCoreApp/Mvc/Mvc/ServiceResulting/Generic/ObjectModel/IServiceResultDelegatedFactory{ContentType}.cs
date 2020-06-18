

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ContentType>
    {
        IServiceResultPostConfiguration ToSuccess();
        IServiceResultPostConfiguration ToSuccess([AllowNull] ContentType content);
        IServiceResultPostConfiguration ToFailure();
        IServiceResultPostConfiguration ToFailure(IServiceResult? serviceResult);
        IServiceResultPostConfiguration ToFailure(JsonErrors? error);
        IServiceResultPostConfiguration ToFailure(JsonError? errors);
        IServiceResultPostConfiguration ToFailure(Exception? error);
        IServiceResultPostConfiguration ToFailure(string? errorMessage);
    }
}
