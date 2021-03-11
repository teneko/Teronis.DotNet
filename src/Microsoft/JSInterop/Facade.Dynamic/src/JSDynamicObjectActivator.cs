using System;
using System.Reflection;
using ImpromptuInterface;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.Dynamic
{
    public static class JSDynamicObjectActivator
    {
        private static void CheckInterfaceType(Type interfaceType)
        {
            if (interfaceType is null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new NotSupportedException("Only interface type is allowed to be proxied.");
            }
        }

        private static void CheckMethodInfo(MethodInfo methodInfo)
        {
            // TODO
        }

        private static MethodDictionary CreateMethodDictionary(params Type[] interfaceTypes)
        {
            var methodDictionary = new MethodDictionary();

            foreach (var interfaceType in interfaceTypes) {
                CheckInterfaceType(interfaceType);

                foreach (var methodInfo in JSFacadeUtils.GetDynamicObjectInterfaceMethods(interfaceType)) {
                    CheckMethodInfo(methodInfo);

                    var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                    parameterList.ThrowParameterListExceptionWhenHavingErrors();

                    var valueTaskType = ValueTaskType.Parse(methodInfo.ReturnType);

                    methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
                }
            }

            return methodDictionary;
        }

        public static T CreateInstance<T>(IJSObjectReference jsObjectReference)
            where T : class, IJSDynamicObject
        {
            var mainInterfaceType = typeof(T);
            var methods = CreateMethodDictionary(mainInterfaceType);
            var jsDynamicObject = new JSDynamicObject(jsObjectReference, methods);
            return jsDynamicObject.ActLike<T>(/*other interfaces*/);
        }
    }
}
