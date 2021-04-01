// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public class DynamicFacadeActivator<TFacadeActivator, TFacade, TCreationOptions> : DynamicFacadeActivatorBase<TFacade>
        where TFacade : IJSObjectReferenceFacade
        where TCreationOptions : DynamicProxyCreationOptions
    {
        private readonly TFacadeActivator activator;

        public DynamicFacadeActivator(TFacadeActivator activator, IJSDynamicProxyActivator jsDynamicProxyActivator, ILateInterceptorBuilder? interceptorBuilder)
            : base(jsDynamicProxyActivator, interceptorBuilder)
        {
            this.activator = activator;
        }

        public ValueTask<TFacade> CreateInstanceAsync(Type interfaceToBeProxied, Func<TFacadeActivator, ValueTask<TFacade>> activateFacade, [AllowNull]TCreationOptions creationOptions) =>
            CreateInstanceAsync(interfaceToBeProxied, () => activateFacade(activator), creationOptions);
    }
}
