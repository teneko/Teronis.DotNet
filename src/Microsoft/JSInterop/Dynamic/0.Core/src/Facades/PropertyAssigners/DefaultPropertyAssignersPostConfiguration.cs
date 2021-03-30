// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Facades;

namespace Teronis.Microsoft.JSInterop.Dynamic.Facades.PropertyAssigners
{
    internal class DefaultPropertyAssignersPostConfiguration : IPostConfigureOptions<JSFacadeHubActivatorOptions>
    {
        public void PostConfigure(string name, JSFacadeHubActivatorOptions options)
        {
            if (!options.AreDefaultPropertyAssigners()) {
                return;
            }

            options.PropertyAssignerFactories.AddDefaultDynamicPropertyAssigners();
        }
    }
}
