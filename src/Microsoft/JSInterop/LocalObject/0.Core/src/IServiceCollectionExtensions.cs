using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.LocalObject
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSLocalObjectActivator(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null) {
            services.TryAddSingleton<IConfigureOptions<JSLocalObjectActivatorOptions>>(serviceProvider => 
                JSFunctionalObjectOptionsConfiguration.Create(serviceProvider));

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSLocalObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }
    }
}
