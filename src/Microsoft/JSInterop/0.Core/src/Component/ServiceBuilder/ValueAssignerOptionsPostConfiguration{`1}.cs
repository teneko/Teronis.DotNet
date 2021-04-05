// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    internal class ValueAssignerOptionsPostConfiguration<TDerived> : IPostConfigureOptions<TDerived>
        where TDerived : ValueAssignerOptions<TDerived>
    {
        private readonly GlobalValueAssignerOptions globalOptions;

        public ValueAssignerOptionsPostConfiguration(IOptions<GlobalValueAssignerOptions> globalOptions) =>
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
