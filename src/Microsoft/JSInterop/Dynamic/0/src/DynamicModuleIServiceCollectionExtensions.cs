// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorServicesOptionsIServiceCollectionExtensions.AddDynamicInterceptorServicesOptions{TInterceptorServicesOptions, TAssignerServicesOptions}(IServiceCollection)"/>,
        /// <see cref="ModuleIServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, System.Action{JSModuleValueAssignerServicesOptions}?, System.Action{JSModuleInterceptorServicesOptions}?, Teronis.Microsoft.JSInterop.Interception.ServiceBuilder.PostConfigureInterceptorServicesOptionsDelegate{JSModuleInterceptorServicesOptions, JSModuleValueAssignerServicesOptions}?)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxyActivator(IServiceCollection, System.Action{Dynamic.JSDynamicProxyValueAssignerServicesOptions}?, System.Action{Dynamic.JSDynamicProxyInterceptorServicesOptions}?, Teronis.Microsoft.JSInterop.Interception.ServiceBuilder.PostConfigureInterceptorServicesOptionsDelegate{Dynamic.JSDynamicProxyInterceptorServicesOptions, Dynamic.JSDynamicProxyValueAssignerServicesOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModuleActivator(this IServiceCollection services)
        {
            services.AddDynamicInterceptorServicesOptions<JSModuleInterceptorServicesOptions, JSModuleValueAssignerServicesOptions>();
            services.AddJSModuleActivator();
            services.AddJSDynamicProxyActivator();
            services.TryAddSingleton<IJSDynamicModuleActivator, JSDynamicModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxy(IServiceCollection)"/>
        /// and <see cref="AddJSDynamicModuleActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModule(this IServiceCollection services)
        {
            services.AddJSModule();
            services.AddJSDynamicProxy();
            services.AddJSDynamicModuleActivator();
            return services;
        }
    }
}
