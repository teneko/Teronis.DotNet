using System;
using Teronis.Extensions;

namespace Teronis.Identity.Presenters
{
    public class JsonError : IJsonError
    {
        internal const string DefaultErrorCode = "error";
        internal const string DefaultErrorMessage = "An exception has been occured.";

        public string ErrorCode { get; private set; }
        public Exception Error { get; private set; }

        public JsonError(string? errorCode, Exception? error)
        {
            ErrorCode = errorCode ?? error?.GetType().Name.LowerFirstLetter() ?? DefaultErrorCode;
            Error = error ?? new Exception(DefaultErrorMessage);
        }

        public JsonError(Exception? error)
            : this(null, error) { }

        public JsonError(string? errorCode, string? error)
            : this(errorCode, error is null ? null : new Exception(error)) { }

        public static implicit operator JsonError(string? errorMessage) =>
            new JsonError(null, errorMessage);

        public override string ToString()
        {
            if (Error is null) {
                return ErrorCode;
            } else {
                return $"{ErrorCode}: {Error}";
            }
        }
    }
}
