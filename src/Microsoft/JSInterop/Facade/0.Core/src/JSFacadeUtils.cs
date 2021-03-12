using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class JSFacadeUtils
    {
        public static IEnumerable<PropertyInfo> GetComponentProperties(Type componentType)
        {
            var propertyInfos = componentType.GetProperties(JSFacadeDefaults.COMPONENT_PROPERTY_BINDING_FLAGS);

            foreach (var propertyInfo in propertyInfos) {
                if (!propertyInfo.CanWrite) {
                    continue;
                }

                yield return propertyInfo;
            }
        }
    }
}
