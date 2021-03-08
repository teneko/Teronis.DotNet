using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSObjectReference module)
        where T : class, IAsyncDisposable;
}
