using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Module
{
    public interface IJSModule : IJSLocalObject
    {
        public string ModuleNameOrPath { get; }
    }
}
