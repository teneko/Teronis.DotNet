// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Teronis.Mvc.ServiceResulting.Generic
{
    public interface IServiceResult<out ContentType> : IServiceResult
    {
        [MaybeNull]
        new ContentType Content { get; }
    }
}
