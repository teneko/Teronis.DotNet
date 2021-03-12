using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.LocalObject.WebAssets;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="IJSLocalObjectInterop"/>> and <see cref="IJSLocalObjectActivator"/> as singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObject(this IServiceCollection services, Action<JSLocalObjectActivatorOptions> configureOptions)
        {
            services.AddOptions<JSLocalObjectActivatorOptions>();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JSLocalObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddJSLocalObjectActivator();
            services.AddJSLocalObjectInterop();
            return services;
        }
    }
}
