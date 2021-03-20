using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic.Module
{
    public static class IJSDynamicModuleActivatorExtensions
    {
        public static async ValueTask<T> CreateInstanceAsync<T>(this IJSDynamicModuleActivator jsDynamicModuleActivator, string moduleNameOrPath)
            where T : class, IJSModule =>
            (T)await jsDynamicModuleActivator.CreateInstanceAsync(typeof(T), moduleNameOrPath);
    }
}
