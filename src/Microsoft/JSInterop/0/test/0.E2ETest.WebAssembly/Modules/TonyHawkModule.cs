// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis_._Microsoft.JSInterop.Modules
{
    [JSModuleClass("./js/es-modules/getTonyHawk.js")]
    public class TonyHawkModule : IAsyncDisposable
    {
        private readonly IJSObjectReferenceFacade jsObjectReferenceFacade;

        public TonyHawkModule(IJSModule jsObject) =>
            jsObjectReferenceFacade = jsObject ?? throw new ArgumentNullException(nameof(jsObject));

        public ValueTask<string> GetTonyHawkAsync() =>
            jsObjectReferenceFacade.InvokeAsync<string>("getTonyHawk");

        public ValueTask DisposeAsync() =>
            jsObjectReferenceFacade.DisposeAsync();
    }
}
