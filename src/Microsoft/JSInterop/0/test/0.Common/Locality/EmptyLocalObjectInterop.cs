// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public class EmptyLocalObjectInterop : IJSLocalObjectInterop
    {
        public ValueTask<IJSObjectReference> GetGlobalObjectReference(string? objectName) =>
            new ValueTask<IJSObjectReference>(new EmptyObjectReference(nameof(EmptyLocalObjectInterop)));

        public ValueTask<IJSObjectReference> GetLocalObjectReference(IJSObjectReference objectReference, string objectName) =>
            new ValueTask<IJSObjectReference>(new EmptyObjectReference(nameof(EmptyLocalObjectInterop)));
    }
}
