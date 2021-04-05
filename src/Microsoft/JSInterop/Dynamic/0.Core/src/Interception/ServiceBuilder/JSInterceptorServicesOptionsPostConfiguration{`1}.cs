// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public sealed class DynamicJSInterceptorServicesOptionsPostConfiguration<TInterceptorServicesOptions> : IPostConfigureOptions<TInterceptorServicesOptions>
        where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
    {
        private readonly JSGlobalInterceptorServicesOptions globalOptions;

        public DynamicJSInterceptorServicesOptionsPostConfiguration(IOptions<JSGlobalInterceptorServicesOptions> globalOptions) =>
            this.globalOptions = globalOptions?.Value ?? throw new ArgumentNullException(nameof(globalOptions));

        public void PostConfigure(string _, TInterceptorServicesOptions options)
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
