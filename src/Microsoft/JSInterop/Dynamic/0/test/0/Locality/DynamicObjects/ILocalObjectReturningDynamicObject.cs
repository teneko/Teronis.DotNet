// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Locality.DynamicObjects
{
    public interface ILocalObjectReturningDynamicObject : IJSModule
    {
        [ReturnLocalObject]
        public ValueTask<IJSLocalObject> GetLocalObject();
    }
}
