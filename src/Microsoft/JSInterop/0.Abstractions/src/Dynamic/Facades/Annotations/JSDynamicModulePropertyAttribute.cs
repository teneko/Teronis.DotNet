using Teronis.Microsoft.JSInterop.Facades.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.Annotations
{
    public class JSDynamicModulePropertyAttribute : JSModulePropertyAttribute
    {
        public JSDynamicModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }

        public JSDynamicModulePropertyAttribute()
        { }
    }
}
