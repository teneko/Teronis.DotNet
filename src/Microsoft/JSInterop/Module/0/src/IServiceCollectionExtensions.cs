using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Module
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModuleActivator(this IServiceCollection services, Action<JSModuleActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSModuleActivatorOptions>>(serviceProvider =>
                JSFunctionalObjectOptionsConfiguration<JSModuleActivatorOptions>.Create(serviceProvider));

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSModuleActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSModuleActivator, JSModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSModuleActivator(IServiceCollection, Action{JSModuleActivatorOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModule(this IServiceCollection services)
        {
            AddJSModuleActivator(services);
            return services;
        }
    }
}
