using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Teronis.Identity.Extensions
{
    public static class ObjectResultExtensions
    {
        [return: NotNullIfNotNull("objectResult")]
        public static T WithHttpStatusCode<T>(this T objectResult, HttpStatusCode statusCode)
            where T : ObjectResult?
        {
            if (!ReferenceEquals(objectResult, null)) {
                objectResult.StatusCode = (int)statusCode;
            }

            return objectResult;
        }
    }
}
