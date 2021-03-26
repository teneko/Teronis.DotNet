// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Dynamic.Locality;
using Teronis.Microsoft.JSInterop.Dynamic.Module;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    /// <inheritdoc/>
    public class JSDynamicFacadeActivators : JSFacadeActivators, IJSFacadeActivators
    {
        public IJSDynamicModuleActivator JsDynamicModuleActivator { get; }
        public IJSDynamicLocalObjectActivator JsDynamicLocalObjectActivator { get; }

        public JSDynamicFacadeActivators(
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSFacadesActivator jsFacadesActivator,
            IJSDynamicModuleActivator jsDynamicModuleActivator,
            IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator)
            : base(jsModuleActivator, jsLocalObjectActivator, jsFacadesActivator)
        {
            JsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));
            JsDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));

            // Prepare instance activators.
            PrepareInstanceActivators(
                jsDynamicModuleActivator,
                jsDynamicLocalObjectActivator);
        }
    }
}
