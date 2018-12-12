using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Extensions
{
    public static class AttributeVariableInfoReceiverExtensions
    {
         public static void LetAttributesReceiveAttributeVariableInfo<T>(this AttributeVariableInfo<T> attrVarInfo) where T : Attribute, IAttrVarInfoReceiver<T> {
                foreach (var attribute in attrVarInfo.Attributes)
                    attribute.ReceiveAttrVarInfo(attrVarInfo);
        }
    }
}
