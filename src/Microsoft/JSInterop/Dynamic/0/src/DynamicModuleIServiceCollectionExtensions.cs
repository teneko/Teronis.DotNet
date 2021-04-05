// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorBuilderOptionsIServiceCollectionExtensions.AddDynamicInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection)"/>,
        /// <see cref="ModuleIServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, System.Action{JSModuleValueAssignerOptions}?, System.Action{JSModuleInterceptorBuilderOptions}?, Interception.PostConfigureInterceptorBuilderDelegate{JSModuleInterceptorBuilderOptions, JSModuleValueAssignerOptions}?)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxyActivator(IServiceCollection, System.Action{Dynamic.JSDynamicProxyValueAssignerOptions}?, System.Action{Dynamic.JSDynamicProxyInterceptorBuilderOptions}?, Interception.PostConfigureInterceptorBuilderDelegate{Dynamic.JSDynamicProxyInterceptorBuilderOptions, Dynamic.JSDynamicProxyValueAssignerOptions}?)"/>,
        /// and tries to add <see cref="JSDynamicModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModuleActivator(this IServiceCollection services)
        {
            services.AddDynamicInterceptorBuilderOptions<JSModuleInterceptorBuilderOptions, JSModuleValueAssignerOptions>();
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
