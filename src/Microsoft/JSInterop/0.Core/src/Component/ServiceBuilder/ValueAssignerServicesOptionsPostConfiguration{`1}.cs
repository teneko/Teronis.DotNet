// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    internal class ValueAssignerServicesOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : ValueAssignerServicesOptions<TDerived>
    {
        private readonly GlobalValueAssignerServicesOptions globalOptions;

        public ValueAssignerServicesOptionsPostConfiguration(IOptions<GlobalValueAssignerServicesOptions> globalOptions) =>
            this.globalOptions = globalOptions.Value ?? throw new System.ArgumentNullException(nameof(globalOptions));

        public void PostConfigure(string name, TDerived options)
        {
            if (!options.TryCreateValueAssignerServicesWhenUserUntouched()) {
                return;
            }

            if (globalOptions.AreValueAssignerServicesUserTouched) {
                options.Services.UseExtension(extension =>
                    extension.Add(globalOptions.Services));
            } else {
                options.Services
                    .AddNonDynamicDefaultValueAssigners()
                    .AddJSCustomFacadeAssigner();
            }
        }
    }
}
