// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component
{
    public static class DynamicIPropertyAssignerOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicPropertyAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddPropertyAssignerOptions<TDerivedAssignerOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedAssignerOptions>,
                DynamicPropertyAssignerOptionsPostConfiguration<TDerivedAssignerOptions>>();

            return services;
        }
    }
}
