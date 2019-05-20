using System;
using System.Linq;
using System.Collections.Generic;
using Teronis.Libraries.NetStandard;

namespace Teronis.Reflection
{
    public class AttributeVariableInfo<T> where T : Attribute
    {
        public VariableInfo VariableInfo { get; private set; }
        public List<T> Attributes { get; private set; }
        public T Attribute => Attributes[0];

        public AttributeVariableInfo(VariableInfo variableInfo, bool? getCustomAttributesInherit)
        {
            VariableInfo = variableInfo;
            var inherit = getCustomAttributesInherit ?? Library.GetCustomAttributesInherit;
            Attributes = variableInfo.GetCustomAttributes(typeof(T), inherit).Select(x => (T)x).ToList();
        }

        public AttributeVariableInfo(VariableInfoType origin, string name, Type valueType, Type declaringType, GetValueHandler getValue, SetValueHandler setValue, IsDefinedHandler isDefined, GetCustomAttributesByTypeHandler getCustomAttributes, bool attrInherit)
            : this(new VariableInfo(origin, name, valueType, declaringType, getValue, setValue, isDefined, getCustomAttributes), attrInherit) { }
    }
}
