// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;

namespace Teronis.Microsoft.JSInterop
{
    public class DynamicFacadeActivator<TFacadeActivator, TFacade, TCreationOptions> : DynamicFacadeActivatorBase<TFacade>
        where TFacade : IJSObjectReferenceFacade
        where TCreationOptions : DynamicProxyCreationOptions
    {
        private readonly TFacadeActivator facadeActivator;

        public DynamicFacadeActivator(TFacadeActivator facadeActivator, IJSDynamicProxyActivator dynamicProxyActivator, IJSInterceptorBuilder? interceptorBuilder)
            : base(dynamicProxyActivator, interceptorBuilder) =>
            this.facadeActivator = facadeActivator;

        public ValueTask<TFacade> CreateInstanceAsync(Type interfaceToBeProxied, Func<TFacadeActivator, ValueTask<TFacade>> activateFacade, [AllowNull]TCreationOptions creationOptions) =>
            CreateInstanceAsync(interfaceToBeProxied, () => activateFacade(facadeActivator), creationOptions);
    }
}
