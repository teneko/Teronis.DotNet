// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorBuilderOptionsIServiceCollectionExtensions.AddDynamicInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection)"/>,
        /// <see cref="JSInterceptorBuilderOptionsIServiceCollectionExtensions.ConfigureInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection, Action{TDerivedAssignerOptions}?, Action{TDerivedBuilderOptions}?, ConfigureMutableInterceptorBuilderDelegate{TDerivedBuilderOptions, TDerivedAssignerOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicProxyActivator"/> as <see cref="IJSDynamicProxyActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureValueAssignerOptions"></param>
        /// <param name="configureInterceptorBuilderOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicProxyActivator(this IServiceCollection services, 
            Action<JSDynamicProxyValueAssignerOptions>? configureValueAssignerOptions = null,
            Action<JSDynamicProxyInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            ConfigureMutableInterceptorBuilderDelegate<JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            services.AddDynamicInterceptorBuilderOptions<JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyValueAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configureValueAssignerOptions, configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSDynamicProxyActivator, JSDynamicProxyActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSDynamicProxyActivator(IServiceCollection, Action{JSDynamicProxyValueAssignerOptions}?, Action{JSDynamicProxyInterceptorBuilderOptions}?, ConfigureMutableInterceptorBuilderDelegate{JSDynamicProxyInterceptorBuilderOptions, JSDynamicProxyValueAssignerOptions}?)"/>.
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
