// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal sealed class ComponentPropertyCollection : IReadOnlyList<ComponentProperty>
    {
        public static ComponentPropertyCollection Create(Type componentType)
        {
            if (componentType is null) {
                throw new ArgumentNullException(nameof(componentType));
            }

            var collection = new ComponentPropertyCollection();
            collection.CollectComponentProperties(componentType);
            return collection;
        }

        public int Count =>
            componentProperties.Count;

        private List<ComponentProperty> componentProperties;

        public ComponentPropertyCollection() =>
            componentProperties = new List<ComponentProperty>();

        public ComponentProperty this[int index] =>
            componentProperties[index];

        private void AddProperty(PropertyInfo propertyInfo)
        {
            var componentProperty = new ComponentProperty(propertyInfo);
            componentProperties.Add(componentProperty);
        }

        private void CollectComponentProperties(Type componentType)
        {
            foreach (var propertyInfo in ComponentPropertyCollectionUtils.GetComponentProperties(componentType)) {
                AddProperty(propertyInfo);
            }
        }

        public IEnumerator<ComponentProperty> GetEnumerator() =>
            componentProperties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
