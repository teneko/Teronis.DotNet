using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module.Dynamic
{
    public interface IJSDynamicModuleActivator : IJSModuleActivator
    {
        ValueTask<T> CreateDynamicInstanceAsync<T>(string moduleNameOrPath)
            where T : class, IJSModule;
    }
}
