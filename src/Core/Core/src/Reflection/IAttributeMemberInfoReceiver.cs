using System;
using Teronis.Reflection;

namespace Teronis
{
    public interface IAttributeMemberInfoReceiver<T> where T : Attribute
    {
        void ReceiveAttributeVariableInfo(AttributeMemberInfo<T> attrVarInfo);
    }
}
