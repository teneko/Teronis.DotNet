// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public static class ValueAssignerOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddValueAssignerOptions<TAssignerOptions>(
            this IServiceCollection services)
            where TAssignerOptions : ValueAssignerOptions<TAssignerOptions>
        {
            services.AddOptions();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TAssignerOptions>,
                ValueAssignerOptionsPostConfiguration<TAssignerOptions>>();

            services.TryAddSingleton<ValueAssignerList<TAssignerOptions>>();
            return services;
        }

        public static IServiceCollection ConfigureValueAssignerOptions<TAssignerOptions>(
            this IServiceCollection services,
            Action<TAssignerOptions>? configureOptions = null)
            where TAssignerOptions : ValueAssignerOptions<TAssignerOptions>
        {
            AddValueAssignerOptions<TAssignerOptions>(services);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            return services;
        }
    }
}
