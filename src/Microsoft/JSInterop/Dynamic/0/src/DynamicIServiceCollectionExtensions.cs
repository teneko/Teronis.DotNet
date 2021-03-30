// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class DynamicIServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicProxyActivator(this IServiceCollection services, Action<JSDynamicProxyActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSDynamicProxyActivatorOptions>>(serviceProvider =>
                JSObjectInterceptorBuilderOptionsConfiguration<JSDynamicProxyActivatorOptions>.Create(serviceProvider));

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSDynamicProxyActivator, JSDynamicProxyActivator>();
            return services;
        }

        public static IServiceCollection AddJSDynamicProxy(this IServiceCollection services) {
            AddJSDynamicProxyActivator(services);
            return services;
        }
    }
}
