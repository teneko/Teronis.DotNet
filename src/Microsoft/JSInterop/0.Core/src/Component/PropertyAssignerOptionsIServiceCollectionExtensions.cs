// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component
{
    public static class PropertyAssignerOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddPropertyAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddOptions();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedAssignerOptions>,
                PropertyAssignerOptionsPostConfiguration<TDerivedAssignerOptions>>();

            return services;
        }

        public static IServiceCollection ConfigurePropertyAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services,
            Action<TDerivedAssignerOptions>? configureOptions = null)
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddPropertyAssignerOptions<TDerivedAssignerOptions>();

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            return services;
        }
    }
}
