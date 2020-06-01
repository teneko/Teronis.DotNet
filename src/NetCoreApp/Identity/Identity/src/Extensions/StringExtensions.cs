using System.Diagnostics.CodeAnalysis;
using Teronis.Identity.Presenters;

namespace Teronis.Identity.Extensions
{
    public static class StringExtensions
    {
        [return: NotNullIfNotNull("errorMessage")]
        [return: NotNullIfNotNull("errorCode")]
        public static JsonError? ToJsonError(this string? errorMessage, string? errorCode = null)
        {
            if (errorMessage is null && errorCode is null) {
                return null;
            }

            return new JsonError(errorMessage, errorCode);
        }

        [return: NotNullIfNotNull("errorMessage")]
        [return: NotNullIfNotNull("errorCode")]
        public static JsonErrors? ToJsonErrors(this string? errorMessage, string? errorCode = null)
        {
            if (errorMessage is null && errorCode is null) {
                return null;
            }

            return new JsonErrors(errorMessage, errorCode);
        }
    }
}
