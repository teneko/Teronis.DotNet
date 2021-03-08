using System;

namespace Teronis.Microsoft.JSInterop.Facade.Annotiations.Design
{
    /// <summary>
    /// Decoratable on class it provides
    /// <see cref="JSModuleFacadeAttributeBase.PathRelativeToWwwRoot"/>
    /// to those properties with facade attribute that are using
    /// <see cref="Annotiations.JSModuleFacadeAttribute.JSModuleFacadeAttribute"/>
    /// but not
    /// <see cref="Annotiations.JSModuleFacadeAttribute.JSModuleFacadeAttribute(string)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JSModuleFacadeAttribute : JSModuleFacadeAttributeBase
    {
        public JSModuleFacadeAttribute(string pathRelativeToWwwRoot)
            : base(pathRelativeToWwwRoot) { }
    }
}
