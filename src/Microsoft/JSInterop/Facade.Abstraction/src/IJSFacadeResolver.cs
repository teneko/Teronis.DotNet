using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeResolver
    {
        ValueTask<IJSLocalObject> CreateModuleReferenceAsync(string relativeWwwRootPath);
        ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType);
        ValueTask<IJSLocalObject> CreateObjectAsync(string objectName);
    }
}
