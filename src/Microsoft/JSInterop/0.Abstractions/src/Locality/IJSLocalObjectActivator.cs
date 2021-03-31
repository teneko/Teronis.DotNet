// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public interface IJSLocalObjectActivator
    {
        IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference);
        ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName);
        ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName);
    }
}
