using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public class JSModuleActivator : IJSModuleActivator
    {
        private readonly IJSRuntime jsRuntime;
        private readonly JSModuleActivatorOptions options;

        public JSModuleActivator(IJSRuntime jsRuntime, JSModuleActivatorOptions options)
        {
            this.jsRuntime = jsRuntime;
            this.options = options;
        }

        public async ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath)
        {
            var jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", moduleNameOrPath);
            return new JSModule(options.GetOrBuildJSFunctionalObject(), jsObjectReference, moduleNameOrPath);
        }
    }
}
