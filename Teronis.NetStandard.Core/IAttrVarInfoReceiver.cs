using System;
using System.Collections.Generic;
using System.Text;
using Teronis.NetStandard.Reflection;

namespace Teronis.NetStandard
{
    public interface IAttrVarInfoReceiver<T> where T : Attribute
    {
        void ReceiveAttrVarInfo(AttributeVariableInfo<T> attrVarInfo);
    }
}
