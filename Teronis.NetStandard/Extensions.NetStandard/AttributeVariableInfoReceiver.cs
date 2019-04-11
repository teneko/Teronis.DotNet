using System;
using Teronis.Reflection;

namespace Teronis.Extensions.NetStandard
{
    public static class AttributeVariableInfoReceiverExtensions
    {
         public static void LetAttributesReceiveAttributeVariableInfo<T>(this AttributeVariableInfo<T> attrVarInfo) where T : Attribute, IAttributeVariableInfoReceiver<T> {
                foreach (var attribute in attrVarInfo.Attributes)
                    attribute.ReceiveAttrVarInfo(attrVarInfo);
        }
    }
}
