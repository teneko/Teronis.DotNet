// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public sealed class DynamicJSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
    {
        private readonly JSGlobalInterceptorBuilderOptions globalOptions;

        public DynamicJSInterceptorBuilderOptionsPostConfiguration(IOptions<JSGlobalInterceptorBuilderOptions> globalOptions) =>
            this.globalOptions = globalOptions?.Value ?? throw new ArgumentNullException(nameof(globalOptions));

        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            if (!options.CreateInterceptorServicesWhenUserUntouched()) {
                return;
            }

            if (globalOptions.AreInterceptorServicesUserTouched) {
                return;
            }

            options.ConfigureInterceptorServices(builder => builder
                .RemoveIterativeValueAssignerInterceptor()
                .AddDefaultDynamicInterceptors()
                .AddIterativeValueAssignerInterceptor());
        }
    }
}
