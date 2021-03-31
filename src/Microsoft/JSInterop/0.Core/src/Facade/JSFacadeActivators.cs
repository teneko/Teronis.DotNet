// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facade
{
    /// <summary>
    /// A default implementation having all activators in one place. You may
    /// derive from it or implement <see cref="IHavingCustomFacadeActivator"/> yourself
    /// to use it in <see cref="IJSFacadeHubActivator"/> to create a more concrete
    /// <see cref="IJSFacadeHub{TJSFacadeActivators}"/>.
    /// </summary>
    public class JSFacadeActivators : IHavingCustomFacadeActivator
    {
        public IJSModuleActivator JSModuleActivator { get; }
        public IJSLocalObjectActivator JSLocalObjectActivator { get; }
        public IJSFacadeHubActivator JSFacadeHubActivator { get; }
        public IJSCustomFacadeActivator JSCustomFacadeActivator { get; }

        public JSFacadeActivators(
            IJSCustomFacadeActivator jsCustomFacadeActivator,
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator,
            IJSFacadeHubActivator jsFacadesActivator)
        {
            JSCustomFacadeActivator = jsCustomFacadeActivator ?? throw new ArgumentNullException(nameof(jsCustomFacadeActivator));
            JSModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            JSLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));
            JSFacadeHubActivator = jsFacadesActivator ?? throw new ArgumentNullException(nameof(jsFacadesActivator));;
        }
    }
}
