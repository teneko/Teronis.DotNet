// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Internals.Utils;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicProxyActivator : IJSDynamicProxyActivator
    {
        private BuildInterceptorDelegate? buildInterceptor;

        public JSDynamicProxyActivator(IOptions<JSDynamicProxyInterceptorBuilderOptions>? options) =>
            buildInterceptor = options is null ? null : (BuildInterceptorDelegate?)options.Value.BuildInterceptor;

        public JSDynamicProxyActivator()
            : this(options: null) { }

        private void CheckInterfaceType(Type interfaceType)
        {
            if (interfaceType is null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (!interfaceType.IsInterface) {
                throw new NotSupportedException("The type is not an interface.");
            }
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

                    var parameterList = ParameterList.Parse(methodInfo.GetParameters());
                    parameterList.ThrowParameterListExceptionWhenHavingErrors();

                    var valueTaskType = ValueTaskType.Parse(methodInfo.ReturnType);

                    methodDictionary.AddMethod(methodInfo, parameterList, valueTaskType);
                }
            }

            return methodDictionary;
        }

        private IJSObjectInterceptor buildInterceptorOrDefault(DynamicProxyCreationOptions? creationOptions) =>
            buildInterceptor
                ?.Invoke(
                    creationOptions?.ConfigureInterceptorBuilder)
                ?? JSObjectInterceptor.Default;

        public object CreateInstance(
            Type interfaceToBeProxied,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSObjectInterceptor? jsObjectInterceptor = null,
            DynamicProxyCreationOptions? creationOptions = null)
        {
            jsObjectInterceptor ??= buildInterceptorOrDefault(creationOptions);
            var derivedInterfaceTypeSet = TypeUtils.GetInterfaces(interfaceToBeProxied);
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
                jsObjectInterceptor);

            return proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToBeProxied,
                jsDynamicObjectInterceptor);
        }

        public object CreateInstance(Type interfaceToBeProxied, IJSObjectReference jSObjectReference, DynamicProxyCreationOptions? creationOptions = null)
        {
            var jsObjectInterceptor = buildInterceptorOrDefault(creationOptions);
            var jsDynamicObject = new JSDynamicProxy(jSObjectReference, jsObjectInterceptor);
            return CreateInstance(interfaceToBeProxied, jsDynamicObject, jsObjectInterceptor: jsObjectInterceptor, creationOptions: creationOptions);
        }
    }
}
