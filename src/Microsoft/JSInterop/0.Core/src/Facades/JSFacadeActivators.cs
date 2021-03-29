// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    /// <summary>
    /// A default implementation of <see cref="IJSFacadeActivators"/>. You may
    /// derive from it or implement <see cref="IJSFacadeActivators"/> yourself
    /// to use it in <see cref="IJSFacadesActivator"/> to create a concrete
    /// <see cref="IJSFacades{TJSFacadeActivators}"/>.
    /// </summary>
    public class JSFacadeActivators : JSFacadeActivatorsBase
    {
        public IJSModuleActivator JSModuleActivator { get; }
        public IJSLocalObjectActivator JSLocalObjectActivator { get; }
        public IJSFacadesActivator JSFacadesActivator { get; }

        public JSFacadeActivators(
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSFacadesActivator jsFacadesActivator)
        {
            JSModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            JSLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));
            JSFacadesActivator = jsFacadesActivator ?? throw new ArgumentNullException(nameof(jsFacadesActivator));

            // Prepare instance activators.
            PrepareInstanceActivators(
                jsModuleActivator,
                jsLocalObjectActivator,
                jsFacadesActivator);
        }
    }
}
