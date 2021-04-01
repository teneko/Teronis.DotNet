// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class JSDynamicLocalObjectActivator : DynamicFacadeActivator<IJSLocalObjectActivator, IJSLocalObject, JSDynamicLocalObjectCreationOptions>, IJSDynamicLocalObjectActivator
    {
        public JSDynamicLocalObjectActivator(
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSDynamicProxyActivator jSDynamicProxyActivator,
            IOptions<JSLocalObjectInterceptorBuilderOptions>? interceptorBuilderOptions)
            : base(jsLocalObjectActivator, jSDynamicProxyActivator, interceptorBuilderOptions?.Value) { }

        public virtual ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string objectName, JSDynamicLocalObjectCreationOptions? options =null) =>
            CreateInstanceAsync(
                interfaceToBeProxied, 
                activator => activator.CreateInstanceAsync(objectName),
                options);

        public virtual ValueTask<IJSLocalObject> CreateInstanceAsync(
            Type interfaceToBeProxied,
            IJSObjectReference jsObjectReference,
            string objectName,
            JSDynamicLocalObjectCreationOptions? options = null) =>
            CreateInstanceAsync(
                interfaceToBeProxied,
                activator => activator.CreateInstanceAsync(jsObjectReference, objectName),
                options);
    }
}
