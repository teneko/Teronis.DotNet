// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeHub<out TJSFacadeActivators> : IAsyncDisposable
        where TJSFacadeActivators : class
    {
        IReadOnlyList<IAsyncDisposable> ComponentDisposables { get; }
        TJSFacadeActivators Activators { get; }
    }
}
