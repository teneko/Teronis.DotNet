// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Module
{
    public class JSDynamicModuleActivator : DynamicFacadeActivator<IJSModuleActivator, IJSModule, DynamicModuleCreationOptions>, IJSDynamicModuleActivator
    {
        public JSDynamicModuleActivator(
            IJSModuleActivator jsModuleActivator,
            IJSDynamicProxyActivator jsDynamicProxyActivator,
            JSInterceptorBuilder<JSModuleInterceptorServicesOptions>? jsInterceptorBuilder)
        : base(jsModuleActivator, jsDynamicProxyActivator, jsInterceptorBuilder) { }

        public virtual ValueTask<IJSModule> CreateInstanceAsync(Type interfaceToBeProxied, string moduleNameOrPath, DynamicModuleCreationOptions? creationOptions = null) =>
            CreateInstanceAsync(
                interfaceToBeProxied,
                activator => activator.CreateInstanceAsync(moduleNameOrPath),
                creationOptions);
    }
}
