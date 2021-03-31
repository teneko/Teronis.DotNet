// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component
{
    internal class PropertyAssignerOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : PropertyAssignerOptions<TDerived>
    {
        private readonly IServiceProvider serviceProvider;

        public PropertyAssignerOptionsPostConfiguration(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public void PostConfigure(string name, TDerived options) {
            options.SetServiceProvider(serviceProvider);

            if (!options.ArePropertyAssignersUserUntouched()) {
                return;
            }

            options.Factories.AddDefaultPropertyAssigners();
        }
    }
}
