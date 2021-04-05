// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    internal class DynamicValueAssignerOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : ValueAssignerOptions<TDerived>
    {
        private readonly GlobalValueAssignerOptions globalOptions;

        public DynamicValueAssignerOptionsPostConfiguration(IOptions<GlobalValueAssignerOptions> globalOptions) =>
            this.globalOptions = globalOptions.Value ?? throw new System.ArgumentNullException(nameof(globalOptions));

        public void PostConfigure(string name, TDerived options)
        {
            if (!options.TryCreateValueAssignerServicesWhenUserUntouched()) {
                return;
            }

            if (globalOptions.AreValueAssignerServicesUserTouched) {
                return;
            }

            options.Services
                .RemoveJSCustomFacadeAssigner()
                .AddDefaultDynamicValueAssigners()
                .AddJSCustomFacadeAssigner();
        }
    }
}
