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
        private static void CollectComponentProperties(ComponentMemberCollection collection, object? component, Type componentType)
        {
            foreach (var memberInfo in ComponentMemberCollectionUtils.GetComponentMembers(componentType)) {
                collection.AddMember(component, memberInfo);
            }
        }

        public static ComponentMemberCollection Create(object? component, Type componentType)
        {
            if (componentType is null) {
                throw new ArgumentNullException(nameof(componentType));
            }

            var collection = new ComponentMemberCollection();
            CollectComponentProperties(collection, component, componentType);
            return collection;
        }

        public int Count =>
            componentProperties.Count;

        private List<ComponentMember> componentProperties;

        public ComponentMemberCollection() =>
            componentProperties = new List<ComponentMember>();

        public ComponentMember this[int index] =>
            componentProperties[index];

        private void AddMember(object? component, MemberInfo memberInfo)
        {
            var componentMember = ComponentMember.Create(component, memberInfo);
            componentProperties.Add(componentMember);
        }

        public IEnumerator<ComponentMember> GetEnumerator() =>
            componentProperties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
