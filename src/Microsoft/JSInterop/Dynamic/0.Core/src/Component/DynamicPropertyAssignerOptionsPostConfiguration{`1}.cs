// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal class DynamicPropertyAssignerOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : PropertyAssignerOptions<TDerived>
    {
        public void PostConfigure(string name, TDerived options)
        {
            if (!options.TryCreatePropertyAssignerFactoriesUserUntouched()) {
                return;
            }

            options.Factories.AddDefaultDynamicPropertyAssigners();
        }
    }
}
