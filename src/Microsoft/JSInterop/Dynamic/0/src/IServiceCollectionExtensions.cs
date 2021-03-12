using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicObjectActicator(IServiceCollection services, Action<JSDynamicObjectActivatorOptions> configureOptions) {
            services.AddOptions<JSDynamicObjectActivatorOptions>();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JSDynamicObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddSingleton<IJSDynamicObjectActivator, JSDynamicObjectActivator>();
            return services;
        }
    }
}
