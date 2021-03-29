// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModuleActivator : IInstanceActivatorBase<IJSModule>, IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;
        private readonly GetOrBuildJSInterceptableFunctionalObjectDelegate? getOrBuildJSInterceptableFunctionalObject;

        public JSModuleActivator(IJSRuntime jsRuntime, IOptions<JSModuleActivatorOptions>? options)
        {
            this.jsRuntime = jsRuntime;
            getOrBuildJSInterceptableFunctionalObject = options?.Value.GetOrBuildJSInterceptableFunctionalObject;
        }

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);
            var jsFunctionalObject = getOrBuildJSInterceptableFunctionalObject?.Invoke(configureInterceptorWalkerBuilder: null) ?? JSFunctionalObject.Default;
            var jsModule = new JSModule(jsFunctionalObject, jsObjectReference, moduleNameOrPath);
            DispatchInstanceActicated(jsModule);
            return jsModule;
        }
    }
}
