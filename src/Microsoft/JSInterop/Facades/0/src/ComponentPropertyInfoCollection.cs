using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class ComponentPropertyInfoCollection : IReadOnlyList<ComponentPropertyInfo>
    {
        public static ComponentPropertyInfoCollection Create(Type componentType)
        {
            if (componentType is null) {
                throw new ArgumentNullException(nameof(componentType));
            }

            var collection = new ComponentPropertyInfoCollection();
            collection.CollectComponentProperties(componentType);
            return collection;
        }

        public int Count =>
            ((IReadOnlyCollection<ComponentPropertyInfo>)componentProperties).Count;

        private List<ComponentPropertyInfo> componentProperties;

        public ComponentPropertyInfoCollection() =>
            componentProperties = new List<ComponentPropertyInfo>();

        public ComponentPropertyInfo this[int index] =>
            ((IReadOnlyList<ComponentPropertyInfo>)componentProperties)[index];

        private void AddProperty(PropertyInfo propertyInfo)
        {
            var componentProperty = new ComponentPropertyInfo(propertyInfo);
            componentProperties.Add(componentProperty);
        }

        private void CollectComponentProperties(Type componentType)
        {
            foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(componentType)) {
                AddProperty(propertyInfo);
            }
        }

        public IEnumerator<ComponentPropertyInfo> GetEnumerator() => 
            ((IEnumerable<ComponentPropertyInfo>)componentProperties).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
