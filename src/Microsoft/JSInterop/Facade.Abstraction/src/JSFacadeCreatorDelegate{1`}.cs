using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSLocalObjectReference module)
        where T : class, IAsyncDisposable;
}
