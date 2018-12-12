using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard
{
    public interface IAttrVarInfoReceiver<T> where T : Attribute
    {
        void ReceiveAttrVarInfo(AttributeVariableInfo<T> attrVarInfo);
    }
}
