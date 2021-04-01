// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Module;
using Teronis.Microsoft.JSInterop.ObjectReferences;

namespace Teronis.Microsoft.JSInterop.Activators
{
    public class EmptyJSModuleActivator : IJSModuleActivator
    {
        public ValueTask<IJSModule> CreateInstanceAsync(string moduleNameOrPath, JSModuleCreationOptions? creationOptions = null) =>
            new ValueTask<IJSModule>(new JSModule(new EmptyObjectReference(), moduleNameOrPath, jsInterceptor: null));
    }
}
