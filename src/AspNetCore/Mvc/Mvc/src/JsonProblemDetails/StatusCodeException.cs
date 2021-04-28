// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
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

