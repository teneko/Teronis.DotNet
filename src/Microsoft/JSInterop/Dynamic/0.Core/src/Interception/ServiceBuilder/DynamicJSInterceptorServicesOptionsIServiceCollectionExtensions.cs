// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public static class DynamicJSInterceptorServicesOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicInterceptorServicesOptions<TInterceptorServicesOptions, TAssignerServicesOptions>(
            this IServiceCollection services)
            where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
            where TAssignerServicesOptions : ValueAssignerServicesOptions<TAssignerServicesOptions>
        {
            services.AddInterceptorServicesOptions<TInterceptorServicesOptions, TAssignerServicesOptions>();
            services.AddDynamicValueAssignerServicesOptions<TAssignerServicesOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TInterceptorServicesOptions>,
                DynamicJSInterceptorServicesOptionsPostConfiguration<TInterceptorServicesOptions>>();

            return services;
        }
    }
}
