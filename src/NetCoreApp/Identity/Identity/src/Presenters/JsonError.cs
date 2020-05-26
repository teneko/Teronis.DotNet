using System;

namespace Teronis.Identity.Presenters
{
    public class JsonError : IJsonError
    {
        internal const string DefaultErrorCode = "error";

        public string ErrorCode { get; private set; }
        public Exception Error { get; private set; }

        public JsonError(string errorCode, Exception error)
        {
            ErrorCode = errorCode;
            Error = error;
        }

        public JsonError(string errorCode, string error)
            : this(errorCode, new Exception(error)) { }

        public static implicit operator JsonError(string errorMessage) =>
            new JsonError(DefaultErrorCode, errorMessage);

        public override string ToString() =>
            $"{ErrorCode}: {Error}";
    }
}
