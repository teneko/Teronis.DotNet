// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSDynamicLocalObjectActivator : DynamicFacadeActivator<IJSLocalObjectActivator, IJSLocalObject, JSDynamicLocalObjectCreationOptions>, IJSDynamicLocalObjectActivator
    {
        public JSDynamicLocalObjectActivator(
            IJSLocalObjectActivator localObjectActivator,
            IJSDynamicProxyActivator dynamicProxyActivator,
            JSInterceptorBuilder<JSLocalObjectInterceptorServicesOptions>? interceptorBuilder)
            : base(localObjectActivator, dynamicProxyActivator, interceptorBuilder) { }

        public virtual ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string globalObjectNameOrPath, JSDynamicLocalObjectCreationOptions? options =null) =>
            CreateInstanceAsync(
                interfaceToBeProxied, 
                activator => activator.CreateInstanceAsync(globalObjectNameOrPath),
                options);

        public virtual ValueTask<IJSLocalObject> CreateInstanceAsync(
            Type interfaceToBeProxied,
            IJSObjectReference objectReference,
            string localObjectNameOrPath,
            JSDynamicLocalObjectCreationOptions? options = null) =>
            CreateInstanceAsync(
                interfaceToBeProxied,
                activator => activator.CreateInstanceAsync(objectReference, localObjectNameOrPath),
                options);
    }
}
