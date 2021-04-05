// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public static class DynamicIValueAssignerServicesOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicValueAssignerServicesOptions<TAssignerServicesOptions>(
            this IServiceCollection services)
            where TAssignerServicesOptions : ValueAssignerServicesOptions<TAssignerServicesOptions>
        {
            services.AddValueAssignerServicesOptions<TAssignerServicesOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TAssignerServicesOptions>,
                DynamicValueAssignerServicesOptionsPostConfiguration<TAssignerServicesOptions>>();

            return services;
        }
    }
}
