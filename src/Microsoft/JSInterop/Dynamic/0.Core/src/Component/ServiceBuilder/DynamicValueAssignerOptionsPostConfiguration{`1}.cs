// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    internal class DynamicValueAssignerOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : ValueAssignerOptions<TDerived>
    {
        public void PostConfigure(string name, TDerived options)
        {
            if (!options.TryCreateValueAssignerFactoriesUserUntouched()) {
                return;
            }

            options.Services
                .RemoveJSCustomFacadeAssigner()
                .AddDefaultDynamicValueAssigners()
                .AddJSCustomFacadeAssigner();
        }
    }
}
