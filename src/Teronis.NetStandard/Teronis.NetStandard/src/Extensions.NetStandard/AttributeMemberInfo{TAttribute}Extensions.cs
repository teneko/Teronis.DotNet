using System;
using System.Linq;
using Teronis.Reflection;

namespace Teronis.Tools.NetStandard
{
    public static class AttributeMemberInfoGeneric
    {
        public static T FirstAttribute<T>(this AttributeMemberInfo<T> attrVarInfo)
            where T : Attribute
            => attrVarInfo.Attributes.First();
    }
}
