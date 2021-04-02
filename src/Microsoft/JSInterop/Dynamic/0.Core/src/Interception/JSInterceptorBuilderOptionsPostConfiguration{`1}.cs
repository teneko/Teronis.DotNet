// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class DynamicJSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
    {
        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            if (!options.TryCreateInterceptorBuilderUserUntouched()) {
                return;
            }

            options.ConfigureInterceptorBuilder(builder => builder
                .RemoveIterativeValueAssignerInterceptor()
                .AddDefaultDynamicInterceptors()
                .AddIterativeValueAssignerInterceptor());
        }
    }
}
