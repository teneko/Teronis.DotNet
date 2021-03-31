// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class JSInterceptorBuilderOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddInterceptorBuilderOptions<TDerivedBuilderOptions, TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddPropertyAssignerOptions<TDerivedAssignerOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedBuilderOptions>,
                JSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions, TDerivedAssignerOptions>>();

            return services;
        }

        public static IServiceCollection ConfigureInterceptorBuilderOptions<TDerivedBuilderOptions, TDerivedAssignerOptions>(
            this IServiceCollection services,
            Action<TDerivedAssignerOptions>? configurePropertyAssignerOptions = null,
            Action<TDerivedBuilderOptions>? configureInterceptorBuilderOptions = null,
            LateConfigureInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
            where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            if (!(configurePropertyAssignerOptions is null)) {
                services.ConfigurePropertyAssignerOptions(configurePropertyAssignerOptions);
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
