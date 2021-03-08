using System;

namespace Teronis.Microsoft.JSInterop.Facade.Annotiations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JSObjectAttribute : Attribute
    {
        public JSObjectAttribute(string objectName) { 
            
        }
    }
}
