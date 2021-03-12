using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class JSDynamicObjectActivatorDefaults
    {
        public const BindingFlags PROXY_INTERFACE__METHOD_BINDING_FLAGS = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;
    }
}
