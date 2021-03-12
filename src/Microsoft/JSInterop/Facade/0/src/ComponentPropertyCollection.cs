using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public sealed class ComponentPropertyCollection
    {
        public static ComponentPropertyCollection Create(object component)
        {
            var collection = new ComponentPropertyCollection();
            collection.CollectComponentProperties(component);
            return collection;
        }

        private List<ComponentProperty> componentProperties;

        public ComponentPropertyCollection() =>
            componentProperties = new List<ComponentProperty>();

        private void AddProperty(PropertyInfo propertyInfo)
        {
            var componentProperty = new ComponentProperty(propertyInfo);
            componentProperties.Add(componentProperty);
        }

        private void CollectComponentProperties(object component)
        {
            foreach (var propertyInfo in JSFacadeUtils.GetComponentProperties(component.GetType())) {
                AddProperty(propertyInfo);
            }
        }
    }
}
