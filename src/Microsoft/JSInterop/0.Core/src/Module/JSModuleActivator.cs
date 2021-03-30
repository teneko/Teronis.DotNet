// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModuleActivator : InstanceActivatorBase<IJSModule>, IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;
        private readonly GetOrBuildInterceptorDelegate? getOrBuildJSInterceptableFunctionalObject;

        public JSModuleActivator(IJSRuntime jsRuntime, IOptions<JSModuleActivatorOptions>? options)
        {
            this.jsRuntime = jsRuntime;
            getOrBuildJSInterceptableFunctionalObject = options?.Value.GetOrBuildInterceptorMethod;
        }

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);
            var jsObjectInterceptor = getOrBuildJSInterceptableFunctionalObject?.Invoke(configureBuilder: null) ?? JSObjectInterceptor.Default;
            var jsModule = new JSModule(jsObjectInterceptor, jsObjectReference, moduleNameOrPath);
            DispatchInstanceActicated(jsModule);
            return jsModule;
        }
    }
}
