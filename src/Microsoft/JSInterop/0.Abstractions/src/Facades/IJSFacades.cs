using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacades<out TJSFacadeActivators> : IAsyncDisposable
        where TJSFacadeActivators : IJSFacadeActivators
    {
        TJSFacadeActivators Activators { get; }

        IAsyncDisposable CreateCustomFacade(Func<TJSFacadeActivators, IJSObjectReferenceFacade> getCustomFacadeConstructorParameter, Type jsCustomFacadeType);
    }
}
