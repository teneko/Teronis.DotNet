// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public static class JSInterceptorBuilderOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddInterceptorBuilderOptions<TInterceptorBuilderOptions, TValueAssignerOptions>(
            this IServiceCollection services)
            where TInterceptorBuilderOptions : JSInterceptorBuilderOptions<TInterceptorBuilderOptions>
            where TValueAssignerOptions : ValueAssignerOptions<TValueAssignerOptions>
        {
            services.AddValueAssignerOptions<TValueAssignerOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TInterceptorBuilderOptions>,
                JSInterceptorBuilderOptionsPostConfiguration<TInterceptorBuilderOptions, TValueAssignerOptions>>();

            services.TryAddSingleton<JSInterceptorBuilder<TInterceptorBuilderOptions>>();
            return services;
        }

        public static IServiceCollection ConfigureInterceptorBuilderOptions<TInterceptorBuilderOptions, TValueAssignerOptions>(
            this IServiceCollection services,
            Action<TValueAssignerOptions>? configureValueAssignerOptions = null,
            Action<TInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            PostConfigureInterceptorBuilderDelegate<TInterceptorBuilderOptions, TValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
            where TInterceptorBuilderOptions : JSInterceptorBuilderOptions<TInterceptorBuilderOptions>
            where TValueAssignerOptions : ValueAssignerOptions<TValueAssignerOptions>
        {
            AddInterceptorBuilderOptions<TInterceptorBuilderOptions, TValueAssignerOptions>(services);

            if (!(configureValueAssignerOptions is null)) {
                services.ConfigureValueAssignerOptions(configureValueAssignerOptions);
            }

            if (!(configureInterceptorBuilderOptions is null)) {
                services.Configure(configureInterceptorBuilderOptions);
            }

            if (!(lateConfigureInterceptorBuilderOptions is null)) {
                services.AddSingleton(lateConfigureInterceptorBuilderOptions);
            }

            return services;
        }
    }
}
