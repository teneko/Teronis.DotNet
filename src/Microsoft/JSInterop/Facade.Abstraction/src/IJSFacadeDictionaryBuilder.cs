using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeDictionaryBuilder
    {
        IJSFacadeDictionaryBuilder AddModuleWrapper<T>(JSFacadeCreatorDelegate<T> moduleWrapperCreatorHandler) 
            where T : class, IAsyncDisposable;
    }
}
