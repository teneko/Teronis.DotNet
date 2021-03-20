using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSCustomFacadeActivator
    {
        IAsyncDisposable CreateCustomFacade(IJSObjectReferenceFacade customFacadeConstructorParameter, Type jsCustomFacadeType);
    }
}
