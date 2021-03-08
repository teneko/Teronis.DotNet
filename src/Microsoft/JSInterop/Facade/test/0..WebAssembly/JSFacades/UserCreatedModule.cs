using Teronis.Microsoft.JSInterop.Facade;

namespace Teronis_._Microsoft.JSInterop.Facade.JSFacades
{
    public class UserCreatedModule : ServiceProviderCreatedModule
    {
        public UserCreatedModule(IJSLocalObjectReference objectReference)
            : base(objectReference) { }
    }
}
