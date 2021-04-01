// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSDynamicLocalObjectActivator
    {
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string objectName, JSDynamicLocalObjectCreationOptions? options = null);
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, IJSObjectReference jsObjectReference, string objectName, JSDynamicLocalObjectCreationOptions? options = null);
    }
}
