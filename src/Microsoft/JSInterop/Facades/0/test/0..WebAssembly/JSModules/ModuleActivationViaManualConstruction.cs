// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Module;

namespace Teronis_._Microsoft.JSInterop.Facades.JSModules
{
    // JavaScript module path is inherited from base class.
    public class ModuleActivationViaManualConstruction : ModuleActivationViaDependencyInjection
    {
        public ModuleActivationViaManualConstruction(IJSModule jsObjectReference)
            : base(jsObjectReference) { }
    }
}
