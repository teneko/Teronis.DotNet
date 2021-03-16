using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop.Locality
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="IJSLocalObjectInterop"/>> and <see cref="IJSLocalObjectActivator"/> as singletons.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObject(this IServiceCollection services)
        {
            services.AddJSLocalObjectActivator();
            services.AddJSLocalObjectInterop();
            return services;
        }
    }
}
