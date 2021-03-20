using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public delegate T JSCustomFacadeFactoryDelegate<out T>(IJSObjectReferenceFacade jsObjectReferenceFacade)
        where T : class, IAsyncDisposable;
}
