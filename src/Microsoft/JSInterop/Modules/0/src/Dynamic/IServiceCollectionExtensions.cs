using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Modules.Dynamic
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
        /// Calls <see cref="Modules.IServiceCollectionExtensions.AddJSModuleActivator(IServiceCollection, Action{JSModuleActivatorOptions}?)"/>
        /// and <see cref="AddJSDynamicModuleActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicModules(this IServiceCollection services, Action<JSModuleActivatorOptions>? configureOptions = null)
        {
            services.AddJSModuleActivator(configureOptions);
            AddJSDynamicModuleActivator(services);
            return services;
        }
    }
}
