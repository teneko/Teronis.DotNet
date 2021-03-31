// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop.Dynamic.Activators
{
    public interface IJSDynamicLocalObjectActivator
    {
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(Type interfaceToBeProxied, IJSObjectReference jsObjectReference, string objectName);
    }
}
