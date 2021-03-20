using Teronis.Microsoft.JSInterop.Facades.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.Annotations
{
    public class JSDynamicModuleClassAttribute : JSModuleClassAttribute
    {
        public JSDynamicModuleClassAttribute(string ModuleNameOrPath)
            : base(ModuleNameOrPath) { }
    }
}
