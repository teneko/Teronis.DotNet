﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Facades
{
    internal class JSFacadeHubService<TJSFacadeActivators> : JSFacadeHub<TJSFacadeActivators>
        where TJSFacadeActivators : class
    {
        public JSFacadeHubService(IJSCustomFacadeActivator jsCustomFacadeActivator, IOptions<JSFacadeHubActivatorOptions> options)
            : base(jsCustomFacadeActivator, options.Value.CreateFacadeActivators<TJSFacadeActivators>(serviceProvider: null)) { }
    }
}
