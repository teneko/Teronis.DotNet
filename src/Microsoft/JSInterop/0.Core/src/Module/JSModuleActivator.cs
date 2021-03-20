using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSModuleActivator : IInstanceActivatorBase<IJSModule>, IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;
        private readonly GetOrBuildJSFunctionalObjectDelegate getOrBuildJSFunctionalObject;

        public JSModuleActivator(IJSRuntime jsRuntime, JSModuleActivatorOptions? options)
        {
            this.jsRuntime = jsRuntime;
            getOrBuildJSFunctionalObject = options?.GetOrBuildJSFunctionalObject ?? JSFunctionalObject.GetDefault;
        }

        public virtual async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);
            var jsModule = new JSModule(getOrBuildJSFunctionalObject(), jsObjectReference, moduleNameOrPath);
            DispatchInstanceActicated(jsModule);
            return jsModule;
        }
    }
}
