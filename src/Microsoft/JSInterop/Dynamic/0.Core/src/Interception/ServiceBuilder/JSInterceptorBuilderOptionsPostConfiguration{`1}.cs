// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public sealed class DynamicJSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
    {
        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            if (!options.CreateInterceptorServicesWhenUserUntouched()) {
                return;
            }

            options.ConfigureInterceptorServices(builder => builder
                .RemoveIterativeValueAssignerInterceptor()
                .AddDefaultDynamicInterceptors()
                .AddIterativeValueAssignerInterceptor());
        }
    }
}
