

namespace Teronis.Identity.Presenters.ObjectModel
{
    /// <summary>
    /// Provides methods to set service result where creation and injection is abstracted away.
    /// </summary>
    public interface IServiceResultDelegatedFactory<ContentType>
    {
        IServiceResultFactory ToSucceeded();
        IServiceResultFactory ToSucceededWithContent(ContentType content);
        IServiceResultFactory ToFailed();
        IServiceResultFactory ToFailed(IServiceResult serviceResult);
        IServiceResultFactory ToFailedWithJsonError(JsonError error);
        IServiceResultFactory ToFailedWithErrorMessage(string errorMessage);
    }
}
