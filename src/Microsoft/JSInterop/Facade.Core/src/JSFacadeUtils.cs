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

        public static IEnumerable<MethodInfo> GetDynamicObjectInterfaceMethods(Type dynamicObjectInterfaceType) {
            var memberInfos = dynamicObjectInterfaceType.GetMembers(JSFacadeDefaults.PROXY_INTERFACE__METHOD_BINDING_FLAGS);

            foreach (var memberInfo in memberInfos) {
                if (memberInfo.MemberType != MemberTypes.Method) {
                    throw new NotSupportedException("Only methods are permitted.");
                }

                yield return (MethodInfo)memberInfo;
            }
        }
    }
}
