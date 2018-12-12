using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Teronis.NetStandard.Tools;

namespace Teronis.NetStandard
{
    public class VariableInfo
    {
        public VariableInfoType VariableInfoType => OriginalVariableInfo is PropertyInfo ? VariableInfoType.Property : VariableInfoType.Field;
        public string Name { get; private set; }
        /// <summary>
        /// arg1 -> refObj
        /// </summary>
        public Func<object, object> GetValue { get; private set; }
        /// <summary>
        /// arg1 -> refObj, arg2 -> newVal
        /// </summary>
        public Action<object, object> SetValue { get; private set; }
        public Type ValueType { get; private set; }
        public Type DeclaringType { get; private set; }
        /// <summary>
        /// function { type of attribute, inherit , is defined }
        /// </summary>
        public Func<Type, bool, bool> IsDefined { get; private set; }
        public Func<Type, bool, IEnumerable<Attribute>> GetCustomAttributes { get; private set; }

        /// <summary>
        /// It is either a <see cref="FieldInfo"/> or a <see cref="PropertyInfo"/>.
        /// </summary>
        public object OriginalVariableInfo;

        public VariableInfo(object originalVariableInfo, string name, Type valueType, Type declaringType, Func<object, object> getValue, Action<object, object> setValue, Func<Type, bool, bool> isDefined, Func<Type, bool, IEnumerable<Attribute>> getCustomAttributes)
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

        public bool TryGetAttrVarInfo<A>(out AttributeVariableInfo<A> attrVarInfo, bool inherit = Constants.Inherit) where A : Attribute => TypeTools.TryToAttributeVariableInfo(OriginalVariableInfo, out attrVarInfo, inherit);

        public AttributeVariableInfo<A> TryGetAttrVarInfo<A>(bool inherit = Constants.Inherit) where A : Attribute => TypeTools.TryToAttributeVariableInfo<A>(OriginalVariableInfo, inherit);
    }
}
