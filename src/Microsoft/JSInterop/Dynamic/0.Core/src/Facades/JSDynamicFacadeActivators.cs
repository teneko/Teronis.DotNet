// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    /// <inheritdoc/>
    public class JSDynamicFacadeActivators : JSFacadeActivators
    {
        public IJSDynamicProxyActivator JSDynamicProxyActivator { get; }
        public IJSDynamicModuleActivator JSDynamicModuleActivator { get; }
        public IJSDynamicLocalObjectActivator JSDynamicLocalObjectActivator { get; }

        public JSDynamicFacadeActivators(
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSFacadeHubActivator jsFacadesActivator,
            IJSDynamicProxyActivator jsDynamicProxyActivator,
            IJSDynamicModuleActivator jsDynamicModuleActivator,
            IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator)
            : base(jsModuleActivator, jsLocalObjectActivator, jsFacadesActivator)
        {
            JSDynamicProxyActivator = jsDynamicProxyActivator ?? throw new ArgumentNullException(nameof(jsDynamicProxyActivator));
            JSDynamicModuleActivator = jsDynamicModuleActivator ?? throw new ArgumentNullException(nameof(jsDynamicModuleActivator));
            JSDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));
        }
    }
}
