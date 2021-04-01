// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public class CustomModule : IAsyncDisposable
    {
        public IJSModule Module { get; }

        public CustomModule(IJSModule module)  =>
            Module = module;

        public ValueTask DisposeAsync() =>
            Module.DisposeAsync();
    }
}
