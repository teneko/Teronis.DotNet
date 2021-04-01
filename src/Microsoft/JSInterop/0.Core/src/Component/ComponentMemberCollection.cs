// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal sealed class ComponentMemberCollection : IReadOnlyList<ComponentMember>
    {
        public static ComponentMemberCollection Create(Type componentType)
        {
            if (componentType is null) {
                throw new ArgumentNullException(nameof(componentType));
            }

            var collection = new ComponentMemberCollection();
            collection.CollectComponentProperties(componentType);
            return collection;
        }

        public int Count =>
            componentProperties.Count;

        private List<ComponentMember> componentProperties;

        public ComponentMemberCollection() =>
            componentProperties = new List<ComponentMember>();

        public ComponentMember this[int index] =>
            componentProperties[index];

        private void AddMember(MemberInfo memberInfo)
        {
            var componentMember = ComponentMember.Create(memberInfo);
            componentProperties.Add(componentMember);
        }

        private void CollectComponentProperties(Type componentType)
        {
            foreach (var memberInfo in ComponentMemberCollectionUtils.GetComponentMembers(componentType)) {
                AddMember(memberInfo);
            }
        }

        public IEnumerator<ComponentMember> GetEnumerator() =>
            componentProperties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
