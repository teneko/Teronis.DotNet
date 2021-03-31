// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, System.Action{JSModulePropertyAssignerOptions}?, System.Action{JSModuleInterceptorBuilderOptions}?, Interception.LateConfigureInterceptorBuilderDelegate{JSModuleInterceptorBuilderOptions, JSModulePropertyAssignerOptions}?)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxyActivator(IServiceCollection, System.Action{Dynamic.JSDynamicProxyPropertyAssignerOptions}?, System.Action{Dynamic.JSDynamicProxyInterceptorBuilderOptions}?, Interception.LateConfigureInterceptorBuilderDelegate{Dynamic.JSDynamicProxyInterceptorBuilderOptions, Dynamic.JSDynamicProxyPropertyAssignerOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModuleActivator(this IServiceCollection services)
        {
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
