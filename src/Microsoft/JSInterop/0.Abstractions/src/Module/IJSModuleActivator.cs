using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module
{
    public interface IJSModuleActivator : IInstanceActivator<IJSModule>
    {
        ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath);
    }
}
