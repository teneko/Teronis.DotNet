using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public interface IJSDynamicProxyActivator
    {
        object CreateInstance(
            Type interfaceToBeProxied,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSFunctionalObject? jsFunctionalObject = null,
            DynamicProxyCreationOptions? creationOptions = null);

        object CreateInstance(Type interfaceToBeProxied, IJSObjectReference jSObjectReference, DynamicProxyCreationOptions? creationOptions = null);
    }
}
