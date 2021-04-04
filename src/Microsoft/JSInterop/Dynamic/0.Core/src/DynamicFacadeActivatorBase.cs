// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;
using Teronis.Microsoft.JSInterop.Utils;

namespace Teronis.Microsoft.JSInterop
{
    public abstract class DynamicFacadeActivatorBase<TFacade> : InterceptableFacadeActivatorBase
        where TFacade : IJSObjectReferenceFacade
    {
        private readonly IJSDynamicProxyActivator jSDynamicProxyActivator;

        public DynamicFacadeActivatorBase(IJSDynamicProxyActivator jSDynamicProxyActivator, IJSInterceptorBuilder? interceptorBuilder)
            : base(interceptorBuilder) =>
            this.jSDynamicProxyActivator = jSDynamicProxyActivator;

        protected async ValueTask<TFacade> CreateInstanceAsync(
            Type interfaceToBeProxied,
            Func<ValueTask<TFacade>> createFacade,
            DynamicProxyCreationOptions? creationOptions = null)
        {
            TypeUtils.EnsureInterfaceTypeIsAssignaleTo<TFacade>(interfaceToBeProxied);
            var jsFacade = await createFacade();
            var jsInterceptor = jsFacade.Interceptor;

            var jsDynamicLocalObject = (TFacade)jSDynamicProxyActivator.CreateInstance(
                interfaceToBeProxied,
                jsFacade,
                jsInterceptor: jsInterceptor,
                creationOptions: creationOptions);

            return jsDynamicLocalObject;
        }
    }
}
