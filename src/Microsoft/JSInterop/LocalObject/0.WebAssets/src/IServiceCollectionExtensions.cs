using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.LocalObject.WebAssets
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSLocalObjectInterop(this IServiceCollection services)
        {
            services.AddSingleton<IJSLocalObjectInterop, JSLocalObjectInterop>();
            return services;
        }
    }
}
