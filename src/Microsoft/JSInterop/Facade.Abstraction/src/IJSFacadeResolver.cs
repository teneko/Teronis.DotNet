using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeResolver
    {
        ValueTask<IJSLocalObjectReference> CreateModuleReferenceAsync(string relativeWwwRootPath);
        ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType);
        ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(string objectName);
    }
}
