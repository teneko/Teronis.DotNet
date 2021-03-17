using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public interface IJSDynamicProxyActivator
    {
        T CreateInstance<T>(IJSObjectReferenceFacade jsObjectFacadeToBeProxied, IJSFunctionalObject? jsFunctionalObject = null, DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade;

        T CreateInstance<T>(IJSObjectReference jsObjectReference, DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade;
    }
}
