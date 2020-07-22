using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class IEnumerableMemberInfoExtensions
    {
        public static IEnumerable<MemberInfo> Intersect(IEnumerable<MemberInfo> memberInfos, Type entityType, VariableInfoDescriptor? descriptor = null)
        {
            memberInfos = memberInfos ?? throw new ArgumentNullException(nameof(memberInfos));
            entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            descriptor = descriptor.DefaultIfNull(true);

            foreach (var memberInfo in memberInfos) {
                if (entityType.GetVariableMember(memberInfo.Name, descriptor) is null) {
                    continue;
                }

                yield return memberInfo;
            }
        }
    }
}
