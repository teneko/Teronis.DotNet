using System;
using System.Runtime.Serialization;

namespace Teronis.Mvc.JsonProblemDetails
{
    public class StatusCodeException: Exception, IHasProblemDetailsStatusCode
    {
        public int StatusCode { get; }

        protected StatusCodeException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public StatusCodeException(int statusCode) =>
            StatusCode = statusCode;

        public StatusCodeException(int statusCode, string? message) 
            : base(message) =>
            StatusCode = statusCode;

        public StatusCodeException(int statusCode, string? message, Exception? innerException) 
            : base(message, innerException) =>
            StatusCode = statusCode;
    }
}

