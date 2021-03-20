using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    internal static class ComponentPropertyCollectionUtils
    {
        public static IEnumerable<PropertyInfo> GetComponentProperties(Type componentType)
        {
            var propertyInfos = componentType.GetProperties(ComponentPropertyCollectionDefaults.COMPONENT_PROPERTY_BINDING_FLAGS);

            foreach (var propertyInfo in propertyInfos) {
                if (!propertyInfo.CanWrite) {
                    continue;
                }

                yield return propertyInfo;
            }
        }
    }
}
