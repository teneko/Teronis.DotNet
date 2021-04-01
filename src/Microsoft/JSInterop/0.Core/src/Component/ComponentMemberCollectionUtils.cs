// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal static class ComponentMemberCollectionUtils
    {
        public static IEnumerable<MemberInfo> GetComponentMembers(Type componentType)
        {
            var memberInfos = componentType.GetMembers(ComponentMemberCollectionDefaults.COMPONENT_PROPERTY_BINDING_FLAGS);

            foreach (var memberInfo in memberInfos) {
                if (memberInfo is PropertyInfo propertyInfo) {
                    if (!propertyInfo.CanWrite) {

                        continue;
                    }

                    yield return propertyInfo;
                }

                if (memberInfo is FieldInfo) {
                    yield return memberInfo;
                }
            }
        }
    }
}
