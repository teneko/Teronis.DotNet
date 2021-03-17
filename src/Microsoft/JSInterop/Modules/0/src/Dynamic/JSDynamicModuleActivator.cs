using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Modules.Dynamic;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public class JSDynamicModuleActivator : IJSDynamicModuleActivator
    {
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSDynamicProxyActivator jSDynamicProxyActivator;

        public JSDynamicModuleActivator(IJSModuleActivator jsModuleActivator, IJSDynamicProxyActivator jSDynamicProxyActivator)
        {
            this.jsModuleActivator = jsModuleActivator;
            this.jSDynamicProxyActivator = jSDynamicProxyActivator;
        }

        public ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath) =>
            jsModuleActivator.CreateInstanceAsync(moduleNameOrPath);

        public virtual async ValueTask<T> CreateDynamicInstanceAsync<T>(string moduleNameOrPath)
            where T : class, IJSModule
        {
            var jsModule = await CreateInstanceAsync(moduleNameOrPath);
            var jsDynamicModule = jSDynamicProxyActivator.CreateInstance<T>(jsModule);
            return jsDynamicModule;
        }
    }
}
