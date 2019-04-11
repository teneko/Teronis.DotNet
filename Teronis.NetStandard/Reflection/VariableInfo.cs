using System;
using System.Reflection;
using Teronis.Libraries.NetStandard;
using Teronis.Tools.NetStandard;

namespace Teronis.Reflection
{
    public class VariableInfo
    {
        public VariableInfoType VariableInfoType => OriginalVariableInfo is PropertyInfo ? VariableInfoType.Property : VariableInfoType.Field;
        public string Name { get; private set; }
        public GetValueHandler GetValue { get; private set; }
        public SetValueHandler SetValue { get; private set; }
        public Type ValueType { get; private set; }
        public Type DeclaringType { get; private set; }
        public IsDefinedHandler IsDefined { get; private set; }
        public GetCustomAttributesByTypeHandler GetCustomAttributes { get; private set; }
        /// <summary>
        /// It is either a <see cref="FieldInfo"/> or a <see cref="PropertyInfo"/>.
        /// </summary>
        public object OriginalVariableInfo;

        public VariableInfo(object originalVariableInfo, string name, Type valueType, Type declaringType, GetValueHandler getValue, SetValueHandler setValue, IsDefinedHandler isDefined, GetCustomAttributesByTypeHandler getCustomAttributes)
        {
            OriginalVariableInfo = originalVariableInfo;
            Name = name;
            ValueType = valueType;
            DeclaringType = declaringType;
            GetValue = getValue;
            SetValue = setValue;
            IsDefined = isDefined;
            GetCustomAttributes = getCustomAttributes;
        }

        public bool TryGetAttrVarInfo<A>(out AttributeVariableInfo<A> attrVarInfo, bool inherit = Library.Inherit) where A : Attribute 
            => TypeTools.TryToAttributeVariableInfo(OriginalVariableInfo, out attrVarInfo, inherit);

        public AttributeVariableInfo<A> TryGetAttrVarInfo<A>(bool inherit = Library.Inherit) where A : Attribute 
            => TypeTools.TryToAttributeVariableInfo<A>(OriginalVariableInfo, inherit);
    }
}
