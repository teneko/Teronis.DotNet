using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class IEnumerableMemberInfoUtils
    {
        public static IEnumerable<MemberInfo> Intersect(this IEnumerable<MemberInfo> memberInfos, Type entityType, VariableInfoDescriptor? descriptor = null) =>
            IEnumerableMemberInfoExtensions.Intersect(memberInfos, entityType, descriptor);
    }
}
