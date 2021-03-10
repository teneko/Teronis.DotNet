using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSLocalObject module)
        where T : class, IAsyncDisposable;
}
