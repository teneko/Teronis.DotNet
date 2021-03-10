using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using Dynamitey;
using ImpromptuInterface;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Facade.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public static class JSObjectProxyActivator
    {
        private static void CheckInterfaceType(Type interfaceType) {
            if (interfaceType is null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new NotSupportedException("Only interface type is allowed being proxied.");
            }
        }

        private static void CheckMethodInfo(MethodInfo methodInfo) { 
            // TODO
        }

        public static T CreateInstance<T>(IJSObjectReference objectReference)
            where T : class, IJSObjectProxy
        {
            var interfaceType = typeof(T);
            CheckInterfaceType(interfaceType);

            var methodDictionary = new MethodDictionary();

            foreach (var methodInfo in JSFacadeUtils.GetProxyInterfaceMethods(interfaceType)) {
                CheckMethodInfo(methodInfo);

                var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                parameterList.ThrowAggregateExceptionWhenHavingErrors();

                var valueTaskType = ValueTaskType.Parse(methodInfo.ReturnType);

                methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
            }

            var jsObjectProxy = new JSObjectProxy(objectReference, methodDictionary);
            return jsObjectProxy.ActLike<T>();
        }
    }
}
