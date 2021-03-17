using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Module
{
    public interface IJSModuleActivator
    {
        ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath);
    }
}
