

using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Identity.Presenters.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ContentType>
    {
        IServiceResultFactory ToSuccess();
        IServiceResultFactory ToSuccess([AllowNull]ContentType content);
        IServiceResultFactory ToFailure();
        IServiceResultFactory ToFailure(IServiceResult? serviceResult);
        IServiceResultFactory ToFailure(JsonError? error);
        IServiceResultFactory ToFailure(Exception? error);
        IServiceResultFactory ToFailure(string? errorMessage);
    }
}
