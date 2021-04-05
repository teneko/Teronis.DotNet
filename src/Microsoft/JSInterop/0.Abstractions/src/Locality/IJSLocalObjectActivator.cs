// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectActivator
    {
        IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference, JSLocalObjectCreationOptions? creationOptions = null);
        ValueTask<IJSLocalObject> CreateInstanceAsync(string globalObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string localObjectNameOrPath, JSLocalObjectCreationOptions? creationOptions = null);
    }
}
