// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Facades.PropertyAssigners
{
    internal class DefaultPropertyAssignersPostConfiguration : IPostConfigureOptions<JSFacadeHubActivatorOptions>
    {
        public void PostConfigure(string name, JSFacadeHubActivatorOptions options) {
            if (!options.ArePropertyAssignersUserUntouched()) {
                return;
            }

            options.PropertyAssignerFactories.AddDefaultPropertyAssigners();
        }
    }
}
