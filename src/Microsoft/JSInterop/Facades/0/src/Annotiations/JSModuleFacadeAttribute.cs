using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotiations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JSModuleFacadeAttribute : JSModuleFacadeAttributeBase
    {
        /// <inheritdoc/>
        public JSModuleFacadeAttribute() 
        { }

        /// <inheritdoc/>
        public JSModuleFacadeAttribute(string moduleNameOrPath)
            : base(moduleNameOrPath) { }
    }
}
