using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public interface IJSModuleActivator
    {
        ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath);
    }
}
