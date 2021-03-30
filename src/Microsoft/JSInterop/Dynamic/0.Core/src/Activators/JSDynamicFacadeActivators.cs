// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Facades;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic.Activators
{
    /// <inheritdoc/>
    public class JSDynamicFacadeActivators : JSFacadeActivators, IJSFacadeActivators
    {
        public IJSDynamicModuleActivator JSDynamicModuleActivator { get; }
        public IJSDynamicLocalObjectActivator JSDynamicLocalObjectActivator { get; }

        public JSDynamicFacadeActivators(
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSFacadesActivator jsFacadesActivator,
            IJSDynamicModuleActivator jsDynamicModuleActivator,
            IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator)
            : base(jsModuleActivator, jsLocalObjectActivator, jsFacadesActivator)
        {
            JSDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));
            JSDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));

            // Prepare instance activators.
            PrepareInstanceActivators(
                jsDynamicModuleActivator,
                jsDynamicLocalObjectActivator);
        }
    }
}
