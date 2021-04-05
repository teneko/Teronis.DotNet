// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public static class ValueAssignerServicesOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddValueAssignerServicesOptions<TAssignerServicesOptions>(
            this IServiceCollection services)
            where TAssignerServicesOptions : ValueAssignerServicesOptions<TAssignerServicesOptions>
        {
            services.AddOptions();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TAssignerServicesOptions>,
                ValueAssignerServicesOptionsPostConfiguration<TAssignerServicesOptions>>();

            services.TryAddSingleton<ValueAssignerList<TAssignerServicesOptions>>();
            return services;
        }

        public static IServiceCollection ConfigureValueAssignerServicesOptions<TAssignerServicesOptions>(
            this IServiceCollection services,
            Action<TAssignerServicesOptions>? configureOptions = null)
            where TAssignerServicesOptions : ValueAssignerServicesOptions<TAssignerServicesOptions>
        {
            AddValueAssignerServicesOptions<TAssignerServicesOptions>(services);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            return services;
        }
    }
}
