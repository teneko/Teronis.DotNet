using System;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeResolver
    {
        ValueTask<IAsyncDisposable> ResolveModule(string pathRelativeToWwwRoot, Type moduleWrapperType);
        //ValueTask<IAsyncDisposable> ResolveObject(string objectName, Type moduleWrapperType);
    }
}
