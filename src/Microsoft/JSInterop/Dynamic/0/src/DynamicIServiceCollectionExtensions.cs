// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorServicesOptionsIServiceCollectionExtensions.AddDynamicInterceptorServicesOptions{TInterceptorServicesOptions, TAssignerServicesOptions}(IServiceCollection)"/>,
        /// <see cref="JSInterceptorServicesOptionsIServiceCollectionExtensions.ConfigureInterceptorServicesOptions{TInterceptorServicesOptions, TAssignerServicesOptions}(IServiceCollection, Action{TAssignerServicesOptions}?, Action{TInterceptorServicesOptions}?, PostConfigureInterceptorServicesOptionsDelegate{TInterceptorServicesOptions, TAssignerServicesOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicProxyActivator"/> as <see cref="IJSDynamicProxyActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureValueAssignerServicesOptions"></param>
        /// <param name="configureInterceptorServicesOptions"></param>
        /// <param name="lateConfigureInterceptorServicesOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicProxyActivator(this IServiceCollection services, 
            Action<JSDynamicProxyValueAssignerServicesOptions>? configureValueAssignerServicesOptions = null,
            Action<JSDynamicProxyInterceptorServicesOptions>? configureInterceptorServicesOptions = null,
            PostConfigureInterceptorServicesOptionsDelegate<JSDynamicProxyInterceptorServicesOptions, JSDynamicProxyValueAssignerServicesOptions>? lateConfigureInterceptorServicesOptions = null)
        {
            services.AddDynamicInterceptorServicesOptions<JSDynamicProxyInterceptorServicesOptions, JSDynamicProxyValueAssignerServicesOptions>();
            services.ConfigureInterceptorServicesOptions(configureValueAssignerServicesOptions, configureInterceptorServicesOptions, lateConfigureInterceptorServicesOptions);
            services.TryAddSingleton<IJSDynamicProxyActivator, JSDynamicProxyActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSDynamicProxyActivator(IServiceCollection, Action{JSDynamicProxyValueAssignerServicesOptions}?, Action{JSDynamicProxyInterceptorServicesOptions}?, PostConfigureInterceptorServicesOptionsDelegate{JSDynamicProxyInterceptorServicesOptions, JSDynamicProxyValueAssignerServicesOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicProxy(this IServiceCollection services)
        {
            services.AddJSDynamicProxyActivator();
            return services;
        }
    }
}
