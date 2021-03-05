using System;
using System.Threading.Tasks;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeResolver
    {
        ValueTask<IAsyncDisposable> ResolveModuleWrapper(string relativeWwwRootPath, Type moduleWrapperType);
    }
}
