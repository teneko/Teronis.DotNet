// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public interface IJSDynamicProxyActivator : IInstanceActivator<IJSObjectReferenceFacade>
    {
        object CreateInstance(
            Type interfaceToBeProxied,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSObjectInterceptor? jsObjectInterceptor = null,
            DynamicProxyCreationOptions? creationOptions = null);

        object CreateInstance(Type interfaceToBeProxied, IJSObjectReference jSObjectReference, DynamicProxyCreationOptions? creationOptions = null);
    }
}
