using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic;

namespace Teronis.Microsoft.JSInterop.Locality.Dynamic
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSDynamicLocalObjectActivator"/> as <see cref="IJSDynamicLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicLocalObjectActivator(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IJSDynamicLocalObjectActivator, JSDynamicLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="Locality.IServiceCollectionExtensions.AddJSLocalObject(IServiceCollection)"/>
        /// and <see cref="AddJSLocalObjectProxy(IServiceCollection, Action{JSLocalObjectActivatorOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectProxy(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null) {
            
            services.AddJSLocalObject();
            services.AddJSDynamic();
            AddJSDynamicLocalObjectActivator(services, configureOptions);
            return services;
        }
    }
}
