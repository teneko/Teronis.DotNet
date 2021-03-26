// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Extensions;
using Teronis.AspNetCore.Identity;

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
            ErrorCode = errorCode ?? error?.GetType().Name.UpperFirst() ?? DefaultErrorCode;
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
