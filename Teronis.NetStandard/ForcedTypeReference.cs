using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ForcedTypeReferenceAttribute : Attribute
    {
        public ForcedTypeReferenceAttribute(Type forcedType)
        {
            // Not sure if these two lines are required since 
            // the type is passed to constructor as parameter, 
            // thus effectively being used
            Action<Type> noop = _ => { };
            noop(forcedType);
        }
    }
}
