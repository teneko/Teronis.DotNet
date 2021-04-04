// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
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
            ConfigureMutableInterceptorBuilderDelegate<TInterceptorBuilderOptions, TValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
            where TInterceptorBuilderOptions : JSInterceptorBuilderOptions<TInterceptorBuilderOptions>
            where TValueAssignerOptions : ValueAssignerOptions<TValueAssignerOptions>
        {
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
