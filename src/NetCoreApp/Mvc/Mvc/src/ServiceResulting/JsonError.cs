using System;
using Teronis.Extensions;
using Teronis.Identity;

namespace Teronis.Mvc.ServiceResulting
{
    public class JsonError : IJsonError
    {
        public const string DefaultErrorCode = "Error";

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
