using System;
using System.Linq;
using System.Collections.Generic;

namespace Teronis.NetStandard.Reflection
{
    public class AttributeVariableInfo<T> where T : Attribute
    {
        public VariableInfo VariableInfo { get; private set; }
        public List<T> Attributes { get; private set; }
        public T Attribute => Attributes[0];

        public AttributeVariableInfo(VariableInfoType origin, string name, Type valueType, Type declaringType, GetValueHandler getValue, SetValueHandler setValue, IsDefinedHandler isDefined, GetCustomAttributesByTypeHandler getCustomAttributes, bool attrInherit) : this(new VariableInfo(origin, name, valueType, declaringType, getValue, setValue, isDefined, getCustomAttributes), attrInherit) { }

        public AttributeVariableInfo(VariableInfo variableInfo, bool attrInherit)
        {
            VariableInfo = variableInfo;
            Attributes = variableInfo.GetCustomAttributes(typeof(T), attrInherit).Select(x => (T)x).ToList();
        }
    }
}
