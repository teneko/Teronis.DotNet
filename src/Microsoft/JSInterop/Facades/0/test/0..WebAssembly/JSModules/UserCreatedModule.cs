using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    public class UserCreatedModule : ServiceProviderCreatedModule
    {
        public UserCreatedModule(IJSLocalObject objectReference)
            : base(objectReference) { }
    }
}
