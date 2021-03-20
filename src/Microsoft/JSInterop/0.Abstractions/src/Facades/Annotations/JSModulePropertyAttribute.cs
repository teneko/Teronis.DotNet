using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JSModulePropertyAttribute : JSModuleAttributeBase
    {
        /// <inheritdoc/>
        public JSModulePropertyAttribute()
        { }

        /// <inheritdoc/>
        public JSModulePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }
    }
}
