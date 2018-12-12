using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Teronis.NetStandard.Tools;

namespace Teronis.NetStandard.Extensions
{
    public static class FieldInfoExtensions
    {
        public static bool TryToVariableInfo(this FieldInfo field, out VariableInfo varInfo)
            => TypeTools.TryToVariableInfo(field, () => field.Name, () => field.FieldType, out varInfo);

        public static bool TryToAttrVarInfo<T>(this FieldInfo field, out AttributeVariableInfo<T> varInfo, bool inherit = Constants.Inherit) where T : Attribute
            => TypeTools.TryToAttributeVariableInfo(field, out varInfo, inherit);
    }
}
