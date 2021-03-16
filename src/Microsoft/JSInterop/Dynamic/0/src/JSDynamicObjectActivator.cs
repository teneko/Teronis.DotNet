using System;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicObjectActivator : IJSDynamicObjectActivator
    {
        private GetOrBuildJSFunctionalObjectDelegate getOrBuildJSFunctionalObjectDelegate;

        public JSDynamicObjectActivator(JSDynamicObjectActivatorOptions? options) =>
            getOrBuildJSFunctionalObjectDelegate = options?.GetOrBuildJSFunctionalObject ?? JSFunctionalObject.GetDefault;

        public JSDynamicObjectActivator()
            : this(options: null) { }

        private void CheckInterfaceType(Type interfaceType)
        {
            if (interfaceType is null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new NotSupportedException("Only interface type is allowed to be proxied.");
            }
        }

        private void CheckMethodInfo(MethodInfo methodInfo)
        {
            // TODO
        }

        private MethodDictionary CreateMethodDictionary(params Type[] interfaceTypes)
        {
            var methodDictionary = new MethodDictionary();

            foreach (var interfaceType in interfaceTypes) {
                CheckInterfaceType(interfaceType);

                foreach (var methodInfo in JSDynamicObjectActivatorUtils.GetDynamicObjectInterfaceMethods(interfaceType)) {
                    CheckMethodInfo(methodInfo);

                    var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                    parameterList.ThrowParameterListExceptionWhenHavingErrors();

                    var valueTaskType = ValueTaskReturnType.Parse(methodInfo.ReturnType);

                    methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
                }
            }

            return methodDictionary;
        }

        public T CreateInstance<T>(IJSObjectReference jsObjectReference)
            where T : class, IJSDynamicObject
        {
            var mainInterfaceType = typeof(T);
            var methodDictionary = CreateMethodDictionary(mainInterfaceType); // The idea is to forward all not lookup methods
            var jsFunctionalObject = getOrBuildJSFunctionalObjectDelegate();
            var jsDynamicObjectProxy = new JSDynamicObjectProxy(jsObjectReference, jsFunctionalObject);
            var proxyGenerator = new ProxyGenerator();

            var jsDynamicObjectInterceptor = new JSDynamicObjectProxyInterceptor(
                jsDynamicObjectProxy,
                methodDictionary,
                jsFunctionalObject);

            return (T)proxyGenerator.CreateInterfaceProxyWithoutTarget(
                mainInterfaceType,
                jsDynamicObjectInterceptor);
        }
    }
}
