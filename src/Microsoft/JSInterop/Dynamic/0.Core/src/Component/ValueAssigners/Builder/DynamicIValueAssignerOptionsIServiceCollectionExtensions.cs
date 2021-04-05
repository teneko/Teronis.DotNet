// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners.Builder
{
    public static class DynamicIValueAssignerOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicValueAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedAssignerOptions : ValueAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddValueAssignerOptions<TDerivedAssignerOptions>();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedAssignerOptions>,
                DynamicValueAssignerOptionsPostConfiguration<TDerivedAssignerOptions>>();

            return services;
        }
    }
}
