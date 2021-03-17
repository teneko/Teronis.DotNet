using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Utils;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicProxyActivator : IJSDynamicProxyActivator
    {
        private GetOrBuildJSFunctionalObjectDelegate getOrBuildJSFunctionalObject;

        public JSDynamicProxyActivator(JSDynamicProxyActivatorOptions? options) =>
            getOrBuildJSFunctionalObject = options?.GetOrBuildJSFunctionalObject ?? JSFunctionalObject.GetDefault;

        public JSDynamicProxyActivator()
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

        private MethodDictionary CreateMethodDictionary(IEnumerable<Type> interfaceTypes, IReadOnlySet<string>? methodsNotProxied = null)
        {
            var methodDictionary = new MethodDictionary();

            foreach (var interfaceType in interfaceTypes) {
                CheckInterfaceType(interfaceType);

                foreach (var methodInfo in JSDynamicProxyActivatorUtils.GetDynamicObjectInterfaceMethods(interfaceType)) {
                    if (!(methodsNotProxied is null) && methodsNotProxied.Contains(methodInfo.Name)) {
                        continue;
                    }

                    CheckMethodInfo(methodInfo);

                    var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                    parameterList.ThrowParameterListExceptionWhenHavingErrors();

                    var valueTaskType = ValueTaskType.Parse(methodInfo.ReturnType);

                    methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
                }
            }

            return methodDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type <typeparamref name="T"/> has to be an interface.</typeparam>
        /// <param name="jsObjectReference">An original reference of <see cref="IJSRuntime"/>.</param>
        /// <param name="jsObjectFacadeToBeProxied">A facade or proxy that implements <see cref="IJSObjectReferenceFacade"/>.</param>
        /// <param name="jsFunctionalObject">The functional object of <paramref name="jsObjectReference"/>.</param>
        /// <param name="creationOptions">The dynamic proxy creation options.</param>
        /// <returns>A proxy that implements <typeparamref name="T"/>.</returns>
        /// <exception cref="NotSupportedException">Only interface type is allowed to be proxied.</exception>
        public T CreateInstance<T>(
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSFunctionalObject? jsFunctionalObject = null,
            DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade
        {
            jsFunctionalObject ??= getOrBuildJSFunctionalObject();

            var mainInterfaceType = typeof(T);

            var derivedInterfaceTypeSet = TypeUtils.GetInterfaces(mainInterfaceType);
            var notDerivedInterfaceTypeSet = TypeUtils.GetInterfaces(typeof(IJSObjectReferenceFacade));
            derivedInterfaceTypeSet.ExceptWith(notDerivedInterfaceTypeSet);

            if (!(creationOptions?.InterfaceTypesNotProxied is null)) {
                derivedInterfaceTypeSet.ExceptWith(creationOptions.InterfaceTypesNotProxied);
            }

            var methodDictionary = CreateMethodDictionary(derivedInterfaceTypeSet, creationOptions?.MethodsNotProxied);

            var proxyGenerator = new ProxyGenerator();

            var jsDynamicObjectInterceptor = new JSDynamicProxyInterceptor(
                jsObjectFacadeToBeProxied,
                methodDictionary,
                jsFunctionalObject);

            return (T)proxyGenerator.CreateInterfaceProxyWithoutTarget(
                mainInterfaceType,
                jsDynamicObjectInterceptor);
        }

        public T CreateInstance<T>(IJSObjectReference jSObjectReference, DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade
        {
            var jsFunctionalObject = getOrBuildJSFunctionalObject();
            var jsDynamicObject = new JSDynamicProxy(jSObjectReference, jsFunctionalObject);
            return CreateInstance<T>(jsDynamicObject, jsFunctionalObject: jsFunctionalObject, creationOptions: creationOptions);
        }
    }
}
