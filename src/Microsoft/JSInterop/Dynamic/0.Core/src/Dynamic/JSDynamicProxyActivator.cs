// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic.Reflection;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Utils;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public class JSDynamicProxyActivator : InterceptableFacadeActivatorBase, IJSDynamicProxyActivator
    {
        public JSDynamicProxyActivator(JSInterceptorBuilder<JSDynamicProxyInterceptorServicesOptions>? interceptorBuilder)
            : base(interceptorBuilder) { }

        public JSDynamicProxyActivator()
            : this(interceptorBuilder: null) { }

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

        public object CreateInstance(
            Type interfaceToBeProxied,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSInterceptor? jsInterceptor = null,
            DynamicProxyCreationOptions? creationOptions = null)
        {
            jsInterceptor ??= BuildInterceptor();
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
                jsInterceptor);

            return proxyGenerator.CreateInterfaceProxyWithoutTarget(
                interfaceToBeProxied,
                jsDynamicObjectInterceptor);
        }

        public object CreateInstance(Type interfaceToBeProxied, IJSObjectReference jSObjectReference, DynamicProxyCreationOptions? creationOptions = null)
        {
            var jsInterceptor = BuildInterceptor();
            var jsDynamicObject = new JSDynamicProxy(jSObjectReference, jsInterceptor);
            return CreateInstance(interfaceToBeProxied, jsDynamicObject, jsInterceptor: jsInterceptor, creationOptions: creationOptions);
        }
    }
}
