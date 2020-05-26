using System;
using Teronis.Identity.Presenters;

namespace Teronis.Identity.Extensions
{
    public static class ExceptionExtensions
    {
        public static JsonError ToJsonError(this Exception error, string? errorCode = null) =>
            new JsonError(errorCode ?? JsonError.DefaultErrorCode, error);
    }
}
