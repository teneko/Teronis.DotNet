// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSDynamicModuleActivator : DynamicFacadeActivator<IJSModuleActivator, IJSModule, DynamicModuleCreationOptions>, IJSDynamicModuleActivator
    {
        public JSDynamicModuleActivator(
            IJSModuleActivator jsModuleActivator,
            IJSDynamicProxyActivator jsDynamicProxyActivator,
            IOptions<JSModuleInterceptorBuilderOptions>? interceptorBuilderOptions) 
        : base(jsModuleActivator, jsDynamicProxyActivator, interceptorBuilderOptions?.Value){ }

        public virtual ValueTask<IJSModule> CreateInstanceAsync(Type interfaceToBeProxied, string moduleNameOrPath, DynamicModuleCreationOptions? creationOptions = null) =>
            CreateInstanceAsync(
                interfaceToBeProxied,
                activator => activator.CreateInstanceAsync(moduleNameOrPath),
                creationOptions);
    }
}
