// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic.Activators;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class DynamicModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add transient <see cref="JSDynamicModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModuleActivator(this IServiceCollection services)
        {
            services.TryAddTransient<IJSDynamicModuleActivator, JSDynamicModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, System.Action{JSModuleInterceptorBuilderOptions}?)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxy(IServiceCollection)"/>
        /// and <see cref="AddJSDynamicModuleActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModule(this IServiceCollection services)
        {
            services.AddJSModuleActivator();
            services.AddJSDynamicProxy();
            services.AddJSDynamicModuleActivator();
            return services;
        }
    }
}
