using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicObject(this IServiceCollection services, Action<JSDynamicObjectActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSDynamicObjectActivatorOptions>>(serviceProvider =>
                JSFunctionalObjectOptionsConfiguration<JSDynamicObjectActivatorOptions>.Create(serviceProvider));

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSDynamicObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSDynamicObjectActivator, JSDynamicObjectActivator>();
            return services;
        }
    }
}
