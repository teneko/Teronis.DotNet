using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Modules
{
    public interface IJSModule : IJSLocalObject
    {
        public string ModuleNameOrPath { get; }
    }
}
