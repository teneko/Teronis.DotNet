// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class DynamicJSInterceptorBuilderOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicInterceptorBuilderOptions<TDerivedBuilderOptions, TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddInterceptorBuilderOptions<TDerivedBuilderOptions, TDerivedAssignerOptions>();
            services.AddDynamicPropertyAssignerOptions<TDerivedAssignerOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedBuilderOptions>,
                DynamicJSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions>>();

            return services;
        }
    }
}
