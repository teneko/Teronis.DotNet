using System;
using Microsoft.JSInterop;

namespace Teronis.AddOn.Microsoft.JSInterop.Facade
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSObjectReference module)
        where T : class, IAsyncDisposable;
}
