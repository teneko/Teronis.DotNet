using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Module.Dynamic
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
        /// Calls <see cref="Module.IServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, Action{JSModuleActivatorOptions}?)"/>
        /// and <see cref="AddJSDynamicModuleActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModule(this IServiceCollection services)
        {
            services.AddJSModuleActivator();
            services.AddJSDynamicProxy();
            AddJSDynamicModuleActivator(services);
            return services;
        }
    }
}
