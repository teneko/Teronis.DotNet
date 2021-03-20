using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    internal class ComponentPropertyCollectionDefaults
    {
        public const BindingFlags COMPONENT_PROPERTY_BINDING_FLAGS = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;
    }
}
