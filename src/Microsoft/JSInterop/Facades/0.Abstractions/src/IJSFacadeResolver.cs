using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.LocalObject;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeResolver
    {
        ValueTask<IJSLocalObject> CreateModuleAsync(string relativeWwwRootPath);
        ValueTask<IAsyncDisposable> ResolveModuleAsync(string pathRelativeToWwwRoot, Type jsFacadeType);
        ValueTask<IJSLocalObject> CreateObjectAsync(string objectName);
    }
}
