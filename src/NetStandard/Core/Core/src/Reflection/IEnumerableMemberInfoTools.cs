using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class IEnumerableMemberInfoExtensions
    {
        public static IEnumerable<MemberInfo> Intersect(this IEnumerable<MemberInfo> memberInfos, Type entityType, VariableInfoDescriptor? descriptor = null) =>
            IEnumerableMemberInfoTools.Intersect(memberInfos, entityType, descriptor);
    }
}
