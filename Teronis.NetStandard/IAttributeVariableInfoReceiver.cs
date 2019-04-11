using System;
using Teronis.Reflection;

namespace Teronis
{
    public interface IAttributeVariableInfoReceiver<T> where T : Attribute
    {
        void ReceiveAttrVarInfo(AttributeVariableInfo<T> attrVarInfo);
    }
}
