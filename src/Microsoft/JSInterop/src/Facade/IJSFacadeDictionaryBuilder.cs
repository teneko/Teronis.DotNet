using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IJSFacadeDictionaryBuilder
    {
        JSFacadeDictionaryBuilder AddModuleWrapper<T>(JSFacadeCreatorDelegate<T> moduleWrapperCreatorHandler) where T : class, IAsyncDisposable;
    }
}