using System;
using System.Linq;

namespace Teronis.Reflection
{
    public static class AttributeMemberInfoExtensions
    {
        public static T FirstAttribute<T>(this AttributeMemberInfo<T> attrVarInfo)
            where T : Attribute
            => attrVarInfo.Attributes.First();
    }
}
