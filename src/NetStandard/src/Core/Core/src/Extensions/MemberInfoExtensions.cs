using System;
using System.Reflection;
using Teronis.Tools;

namespace Teronis.Extensions
{
    public static class MemberInfoExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object owner)
            => MemberInfoTools.GetValue(memberInfo, owner);

        public static void SetValue(this MemberInfo memberInfo, object owner, object value)
            => MemberInfoTools.SetValue(memberInfo, owner, value);

        public static bool IsFieldOrProperty(this MemberInfo memberInfo)
            => MemberInfoTools.IsFieldOrProperty(memberInfo);

        public static Type GetVariableType(this MemberInfo memberInfo)
            => MemberInfoTools.GetVariableType(memberInfo);

        public static bool IsVariable(this MemberInfo memberInfo)
            => MemberInfoTools.IsVariable(memberInfo);

        public static bool IsVariable(this MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => MemberInfoTools.IsVariable(memberInfo, attributeType, getCustomAttributesInherit);
    }
}
