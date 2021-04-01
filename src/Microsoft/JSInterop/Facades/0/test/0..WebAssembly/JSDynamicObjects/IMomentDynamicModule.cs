// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis_._Microsoft.JSInterop.Facades.JSDynamicObjects
{
    public interface IMomentDynamicModule : IJSModule
    {
        [ReturnDynamicProxy]
        ValueTask<IMomentDynamicObject> moment(string date);
    }
}
