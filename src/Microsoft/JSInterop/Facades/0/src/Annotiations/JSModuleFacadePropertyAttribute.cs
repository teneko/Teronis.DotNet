using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotiations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JSModuleFacadePropertyAttribute : JSModuleFacadeAttributeBase
    {
        /// <inheritdoc/>
        public JSModuleFacadePropertyAttribute() 
        { }

        /// <inheritdoc/>
        public JSModuleFacadePropertyAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }
    }
}
