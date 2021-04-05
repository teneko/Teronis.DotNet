// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class IEnumerableMemberInfoUtils
    {
        public static IEnumerable<MemberInfo> Intersect(this IEnumerable<MemberInfo> memberInfos, Type entityType, VariableMemberDescriptor? descriptor = null) =>
            IEnumerableMemberInfoExtensions.Intersect(memberInfos, entityType, descriptor);
    }
}
