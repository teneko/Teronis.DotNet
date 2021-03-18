using Teronis.Microsoft.JSInterop;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    public class UserCreatedModule : ServiceProviderCreatedModule
    {
        public UserCreatedModule(IJSObjectReferenceFacade jsObjectReference)
            : base(jsObjectReference) { }
    }
}
