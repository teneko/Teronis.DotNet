using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.LocalObject.WebAssets
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSLocalObjectInterop(this IServiceCollection services)
        {
            services.TryAddSingleton<IJSLocalObjectInterop, JSLocalObjectInterop>();
            return services;
        }
    }
}
