using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public delegate T JSFacadeCreatorDelegate<out T>(IJSObjectReferenceFacade jsObjectReferenceFacade)
        where T : class, IAsyncDisposable;
}
