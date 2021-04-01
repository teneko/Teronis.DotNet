// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModuleActivator : InterceptableFacadeActivatorBase, IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;

        public JSModuleActivator(IJSRuntime jsRuntime, IOptions<JSModuleInterceptorBuilderOptions>? options)
            : base(options?.Value) =>
            this.jsRuntime = jsRuntime;

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath, JSModuleCreationOptions? creationOptions = null)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);
            var jsInterceptor = BuildInterceptor(creationOptions?.ConfigureInterceptorBuilder);
            var jsModule = new JSModule(jsObjectReference, moduleNameOrPath, jsInterceptor);
            return jsModule;
        }
    }
}
