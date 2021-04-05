// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Teronis.Extensions;
using Teronis.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal static class ComponentMemberCollectionUtils
    {
        public static IEnumerable<MemberInfo> GetComponentMembers(Type componentType)
        {
            var variableDescriptor = new VariableMemberDescriptor() {
                IncludeIfWritable = true,
                Flags = ComponentMemberCollectionDefaults.COMPONENT_PROPERTY_BINDING_FLAGS,
            };

            var memberInfos = componentType.GetVariableMembers(typeof(object), VariableMemberTypes.FieldAndProperty, variableDescriptor);

            foreach (var memberInfo in memberInfos) {
                if (memberInfo is PropertyInfo) {
                    yield return memberInfo;
                }

                if (memberInfo is FieldInfo) {
                    if (memberInfo.Name.StartsWith("<")) {
                        continue;
                    }

                    yield return memberInfo;
                }
            }
        }
    }
}
