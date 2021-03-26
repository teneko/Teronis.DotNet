// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.ServiceResulting
{
    public interface IMutableServiceResult
    {
        bool Succeeded { get; set; }
        object? Content { get; set; }
        int? StatusCode { get; set; }
        JsonErrors? Errors { get; set; }
    }
}
