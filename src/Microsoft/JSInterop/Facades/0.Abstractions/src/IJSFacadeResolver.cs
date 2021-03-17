using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeResolver
    {
        ValueTask<IJSLocalObject> CreateModuleAsync(string moduleNameOrPath);
        ValueTask<IAsyncDisposable> CreateModuleFacadeAsync(string moduleNameOrPath, Type jsFacadeType);
        ValueTask<IJSLocalObject> CreateLocalObjectAsync(string objectName);
    }
}
