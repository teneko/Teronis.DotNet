// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class JSDynamicProxyActivatorUtils
    {
        public static IEnumerable<MethodInfo> GetDynamicObjectInterfaceMethods(Type dynamicObjectInterfaceType)
        {
            var methodInfos = dynamicObjectInterfaceType.GetMethods(JSDynamicProxyActivatorDefaults.PROXY_INTERFACE__METHOD_BINDING_FLAGS);

            foreach (var methodInfo in methodInfos) {
                if (methodInfo.IsSpecialName) {
                    continue; // Skips getter and setter methods of properties.
                }

                yield return methodInfo;
            }
        }
    }
}
