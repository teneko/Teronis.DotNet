using System;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class AttributeMemberInfoReceiverExtensions
    {
        public static void LetAttributesReceiveAttributeMemberInfo<T>(this AttributeMemberInfo<T> attrVarInfo) where T : Attribute, IAttributeMemberInfoReceiver<T>
        {
            foreach (var attribute in attrVarInfo.Attributes) {
                attribute.ReceiveAttributeVariableInfo(attrVarInfo);
            }
        }
    }
}
