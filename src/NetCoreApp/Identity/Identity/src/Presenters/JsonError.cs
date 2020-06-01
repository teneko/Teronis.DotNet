using System;
using Teronis.Extensions;

namespace Teronis.Identity.Presenters
{
    public class JsonError : IJsonError
    {
        internal const string DefaultErrorCode = "Error";

        public string ErrorCode { get; private set; }
        public Exception Error { get; private set; }

        public JsonError(Exception? error = null, string? errorCode = null)
        {
            Error = error ?? new Exception(StringResources.DefaultErrorMessage);
            ErrorCode = errorCode ?? error?.GetType().Name.UpperFirstLetter() ?? DefaultErrorCode;
        }

        public JsonError(Exception? error)
            : this(error, null) { }

        public JsonError(string? errorMessage, string? errorCode = null)
            : this(errorMessage is null ? null : new Exception(errorMessage), errorCode) { }

        public static implicit operator JsonError(string? errorMessage) =>
            new JsonError(errorMessage, null);

        public override string ToString() =>
            $"{Error} (Code: {ErrorCode})";
    }
}
