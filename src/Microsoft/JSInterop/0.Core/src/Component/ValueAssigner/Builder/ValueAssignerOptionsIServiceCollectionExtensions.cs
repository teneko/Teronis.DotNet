// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public static class ValueAssignerOptionsIServiceCollectionExtensions
    {
        public static IServiceCollection AddValueAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services)
            where TDerivedAssignerOptions : ValueAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddOptions();

            services.TryAddTypeUniqueSingleton<
                IPostConfigureOptions<TDerivedAssignerOptions>,
                ValueAssignerOptionsPostConfiguration<TDerivedAssignerOptions>>();

            services.TryAddSingleton<ValueAssignerList<TDerivedAssignerOptions>>();
            return services;
        }

        public static IServiceCollection ConfigureValueAssignerOptions<TDerivedAssignerOptions>(
            this IServiceCollection services,
            Action<TDerivedAssignerOptions>? configureOptions = null)
            where TDerivedAssignerOptions : ValueAssignerOptions<TDerivedAssignerOptions>
        {
            services.AddValueAssignerOptions<TDerivedAssignerOptions>();

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            return services;
        }
    }
}
