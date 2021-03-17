using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public class JSModuleActivator : IJSModuleActivator
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
            return new JSModule(getOrBuildJSFunctionalObject(), jsObjectReference, moduleNameOrPath);
        }
    }
}
