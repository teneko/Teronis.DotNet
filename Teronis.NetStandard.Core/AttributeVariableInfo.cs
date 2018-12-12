using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public class AttributeVariableInfo<T> where T : Attribute
    {
        //public string Name => VarInfo.Name;
        ///// <summary>
        ///// arg1 -> refObj
        ///// </summary>
        //public Func<object, object> GetValue => VarInfo.GetValue;
        ///// <summary>
        ///// arg1 -> refObj, arg2 -> newVal
        ///// </summary>
        //public Action<object, object> SetValue => VarInfo.SetValue;
        //public Type ValueType => VarInfo.ValueType;
        //public Type DeclaringType => VarInfo.DeclaringType;
        public List<T> Attributes { get; private set; }

        public VariableInfo VarInfo { get; private set; }

        public AttributeVariableInfo(VariableInfoType origin, string name, Type valueType, Type declaringType, Func<object, object> getValue, Action<object, object> setValue, Func<Type, bool, bool> isDefined, Func<Type, bool, IEnumerable<Attribute>> getCustomAttributes, bool attrInherit) : this(new VariableInfo(origin, name, valueType, declaringType, getValue, setValue, isDefined, getCustomAttributes), attrInherit) { }

        public AttributeVariableInfo(VariableInfo varInfo, bool attrInherit)
        {
            VarInfo = varInfo;
            Attributes = varInfo.GetCustomAttributes(typeof(T), attrInherit).Select(x => (T)x).ToList();
        }
    }
}
