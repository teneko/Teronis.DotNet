// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorBuilderOptionsIServiceCollectionExtensions.AddDynamicInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection)"/>,
        /// <see cref="JSInterceptorBuilderOptionsIServiceCollectionExtensions.ConfigureInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection, Action{TDerivedAssignerOptions}?, Action{TDerivedBuilderOptions}?, LateConfigureInterceptorBuilderDelegate{TDerivedBuilderOptions, TDerivedAssignerOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicProxyActivator"/> as <see cref="IJSDynamicProxyActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurePropertyAssignerOptions"></param>
        /// <param name="configureInterceptorBuilderOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicProxyActivator(this IServiceCollection services, 
            Action<JSDynamicProxyPropertyAssignerOptions>? configurePropertyAssignerOptions = null,
            Action<JSDynamicProxyInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            LateConfigureInterceptorBuilderDelegate<JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyPropertyAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            services.AddDynamicInterceptorBuilderOptions<JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyPropertyAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configurePropertyAssignerOptions, configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSDynamicProxyActivator, JSDynamicProxyActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSDynamicProxyActivator(IServiceCollection, Action{JSDynamicProxyPropertyAssignerOptions}?, Action{JSDynamicProxyInterceptorBuilderOptions}?, LateConfigureInterceptorBuilderDelegate{JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyPropertyAssignerOptions}?)"/>.
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
