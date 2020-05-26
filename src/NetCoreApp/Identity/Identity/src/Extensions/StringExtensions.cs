using Teronis.Identity.Presenters;

namespace Teronis.Identity.Extensions
{
    public static class StringExtensions
    {
        public static JsonError ToJsonError(this string errorMessage, string? errorCode = null) =>
                new JsonError(errorCode ?? JsonError.DefaultErrorCode, errorMessage);
    }
}
