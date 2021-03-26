// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Mvc.ServiceResulting
{
    public class NotSucceededException : Exception
    {
        public IServiceResult ServiceResult { get; }

        public NotSucceededException(IServiceResult serviceResult) =>
            ServiceResult = serviceResult;

        public NotSucceededException(IServiceResult serviceResult, string? message)
            : base(message) =>
            ServiceResult = serviceResult;

        public NotSucceededException(IServiceResult serviceResult, string? message, Exception? innerException)
            : base(message, innerException) =>
            ServiceResult = serviceResult;
    }
}
