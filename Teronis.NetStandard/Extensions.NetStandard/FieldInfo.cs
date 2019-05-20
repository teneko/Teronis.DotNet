using System;
using System.Reflection;
using Teronis.Libraries.NetStandard;
using Teronis.Reflection;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class FieldInfoExtensions
    {
        public static bool TryToVariableInfo(this FieldInfo field, out VariableInfo varInfo)
            => TypeTools.TryToVariableInfo(field, () => field.Name, () => field.FieldType, out varInfo);

        public static bool TryToAttrVarInfo<T>(this FieldInfo field, out AttributeVariableInfo<T> varInfo, bool? getCustomAttributesInherit = null) where T : Attribute
            => TypeTools.TryToAttributeVariableInfo(field, out varInfo, getCustomAttributesInherit);
    }
}
