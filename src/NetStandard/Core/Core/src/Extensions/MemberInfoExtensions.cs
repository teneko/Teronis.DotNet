// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class MemberInfoExtensions
    {
        public static object? GetValue(this MemberInfo memberInfo, object owner)
            => MemberInfoUtils.GetValue(memberInfo, owner);

        public static void SetValue(this MemberInfo memberInfo, object owner, object? value)
            => MemberInfoUtils.SetValue(memberInfo, owner, value);

        public static bool IsFieldOrProperty(this MemberInfo memberInfo)
            => MemberInfoUtils.IsFieldOrProperty(memberInfo);

        public static Type GetVariableType(this MemberInfo memberInfo)
            => MemberInfoUtils.GetVariableType(memberInfo);

        public static bool IsVariable(this MemberInfo memberInfo)
            => MemberInfoUtils.IsVariable(memberInfo);

        public static bool IsVariable(this MemberInfo memberInfo, Type attributeType, bool getCustomAttributesInherit)
            => MemberInfoUtils.IsVariable(memberInfo, attributeType, getCustomAttributesInherit);
    }
}
