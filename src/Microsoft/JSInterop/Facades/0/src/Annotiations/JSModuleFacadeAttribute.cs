using System;

namespace Teronis.Microsoft.JSInterop.Facades.Annotiations
{
    /// <summary>
    /// Decoratable on class. It provides
    /// <see cref="JSModuleFacadeAttributeBase.ModuleNameOrPath"/>
    /// to those properties with facade attribute that are using
    /// <see cref="JSModuleFacadePropertyAttribute.JSModuleFacadeAttribute"/>
    /// but not
    /// <see cref="JSModuleFacadePropertyAttribute(string)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class JSModuleFacadeAttribute : JSModuleFacadeAttributeBase
    {
        public JSModuleFacadeAttribute(string ModuleNameOrPath)
            : base(ModuleNameOrPath) { }
    }
}
