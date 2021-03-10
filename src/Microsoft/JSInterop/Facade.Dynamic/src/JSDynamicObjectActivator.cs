using System;
using System.Reflection;
using ImpromptuInterface;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public static class JSDynamicObjectActivator
    {
        private static void CheckInterfaceType(Type interfaceType) {
            if (interfaceType is null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new NotSupportedException("Only interface type is allowed to be proxied.");
            }
        }

        private static void CheckMethodInfo(MethodInfo methodInfo) { 
            // TODO
        }

        private static JSDynamicObject CreateInstance(IJSObjectReference jsObjectReference, params Type[] interfaceTypes) {
            var interfaceType = interfaceTypes[0];
            CheckInterfaceType(interfaceType);

            var methodDictionary = new MethodDictionary();

            foreach (var methodInfo in JSFacadeUtils.GetDynamicObjectInterfaceMethods(interfaceType)) {
                CheckMethodInfo(methodInfo);

                var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                parameterList.ThrowAggregateExceptionWhenHavingErrors();

                var valueTaskType = ValueTaskType.Parse(methodInfo.ReturnType);

                methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
            }

            var jsDynamicObject = new JSDynamicObject(jsObjectReference, methodDictionary);
            return jsDynamicObject;
        }

        public static T CreateInstance<T>(IJSObjectReference jsObjectReference)
            where T : class, IJSDynamicObject
        {
            var interfaceType = typeof(T);
            var jsDynamicObject = CreateInstance(jsObjectReference, interfaceType);
            return jsDynamicObject.ActLike<T>();
        }
    }
}
