using Teronis.Microsoft.JSInterop.LocalObject;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    public class UserCreatedModule : ServiceProviderCreatedModule
    {
        public UserCreatedModule(IJSLocalObject objectReference)
            : base(objectReference) { }
    }
}
