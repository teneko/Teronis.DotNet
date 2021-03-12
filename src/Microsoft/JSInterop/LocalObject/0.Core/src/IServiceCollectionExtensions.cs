using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSLocalObjectActivator(this IServiceCollection services) {
            services.AddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }
    }
}
