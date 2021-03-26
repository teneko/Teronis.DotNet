// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Mvc.ServiceResulting
{
    public interface IJsonError
    {
        public string ErrorCode { get; }
        public Exception? Error { get; }
    }
}
