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
        public static IServiceCollection AddJSDynamicProxyActivator(this IServiceCollection services, Action<JSDynamicProxyInterceptorBuilderOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSDynamicProxyInterceptorBuilderOptions>>(serviceProvider =>
                JSObjectInterceptorBuilderOptionsConfiguration<JSDynamicProxyInterceptorBuilderOptions>.Create(serviceProvider));

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddTransient<IJSDynamicProxyActivator, JSDynamicProxyActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSDynamicProxyActivator(IServiceCollection, Action{JSDynamicProxyInterceptorBuilderOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicProxy(this IServiceCollection services) {
            AddJSDynamicProxyActivator(services);
            return services;
        }
    }
}
