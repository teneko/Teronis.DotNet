using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;
using Teronis.Microsoft.JSInterop.Modules;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSLocalObjectActivatorOptions"/> and 
        /// tries to add <see cref="JSLocalObjectActivatorOptions"/> as singleton.
        /// Tries to adds <see cref="JSLocalObjectActivator"/> as <see cref="IJSLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSLocalObjectActivatorOptions>>(serviceProvider =>
                JSFunctionalObjectOptionsConfiguration<JSLocalObjectActivatorOptions>.Create(serviceProvider));

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSLocalObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="WebAssets.IServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectActivatorOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjects(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null)
        {
            services.AddJSModules();
            services.AddJSLocalObjectInterop();
            AddJSLocalObjectActivator(services, configureOptions);
            return services;
        }
    }
}
