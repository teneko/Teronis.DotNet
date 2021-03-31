// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModuleActivator : IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;
        private readonly BuildInterceptorDelegate? buildInterceptor;

        public JSModuleActivator(IJSRuntime jsRuntime, IOptions<JSModuleInterceptorBuilderOptions>? options)
        {
            this.jsRuntime = jsRuntime;
            buildInterceptor = options is null ? null : (BuildInterceptorDelegate?)options.Value.BuildInterceptor;
        }

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);

            var jsObjectInterceptor = buildInterceptor
                ?.Invoke(
                    configureBuilder: null)
                ?? JSObjectInterceptor.Default;

            var jsModule = new JSModule(jsObjectReference, moduleNameOrPath, jsObjectInterceptor);
            return jsModule;
        }
    }
}
