// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public static class JSInterceptorServicesOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddInterceptorServicesOptions<TInterceptorServicesOptions, TValueAssignerServicesOptions>(
            this IServiceCollection services)
            where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
            where TValueAssignerServicesOptions : ValueAssignerServicesOptions<TValueAssignerServicesOptions>
        {
            services.AddValueAssignerServicesOptions<TValueAssignerServicesOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TInterceptorServicesOptions>,
                JSInterceptorServicesOptionsPostConfiguration<TInterceptorServicesOptions, TValueAssignerServicesOptions>>();

            services.TryAddSingleton<JSInterceptorBuilder<TInterceptorServicesOptions>>();
            return services;
        }

        public static IServiceCollection ConfigureInterceptorServicesOptions<TInterceptorServicesOptions, TValueAssignerServicesOptions>(
            this IServiceCollection services,
            Action<TValueAssignerServicesOptions>? configureValueAssignerServicesOptions = null,
            Action<TInterceptorServicesOptions>? configureInterceptorServicesOptions = null,
            PostConfigureInterceptorServicesOptionsDelegate<TInterceptorServicesOptions, TValueAssignerServicesOptions>? lateConfigureInterceptorServicesOptions = null)
            where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
            where TValueAssignerServicesOptions : ValueAssignerServicesOptions<TValueAssignerServicesOptions>
        {
            AddInterceptorServicesOptions<TInterceptorServicesOptions, TValueAssignerServicesOptions>(services);

            if (!(configureValueAssignerServicesOptions is null)) {
                services.ConfigureValueAssignerServicesOptions(configureValueAssignerServicesOptions);
            }

            if (!(configureInterceptorServicesOptions is null)) {
                services.Configure(configureInterceptorServicesOptions);
            }

            if (!(lateConfigureInterceptorServicesOptions is null)) {
                services.AddSingleton(lateConfigureInterceptorServicesOptions);
            }

            return services;
        }
    }
}
