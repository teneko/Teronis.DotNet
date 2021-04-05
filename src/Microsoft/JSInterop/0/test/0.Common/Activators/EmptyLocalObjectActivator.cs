// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop.Activators
{
    public class EmptyLocalObjectActivator : IJSLocalObjectActivator
    {
        public IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference, JSLocalObjectCreationOptions? creationOptions = null) =>
            throw new NotImplementedException();

        public ValueTask<IJSLocalObject> CreateInstanceAsync(string globalObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null) =>
            new ValueTask<IJSLocalObject>(new JSLocalObject(new EmptyObjectReference(nameof(EmptyLocalObjectActivator)), jsInterceptor: null));

        public ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string localObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null) =>
            throw new NotImplementedException();
    }
}
