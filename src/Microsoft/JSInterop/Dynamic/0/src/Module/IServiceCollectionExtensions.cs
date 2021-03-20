using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Dynamic.Module
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSDynamicModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModuleActivator(this IServiceCollection services)
        {
            services.TryAddSingleton<IJSDynamicModuleActivator, JSDynamicModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="IServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, Action{JSModuleActivatorOptions}?)"/>
        /// and <see cref="AddJSDynamicModuleActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
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
