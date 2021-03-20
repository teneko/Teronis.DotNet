using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Facades.ComponentPropertyAssigners;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSCustomFacadeActivator(this IServiceCollection services, Action<JSCustomFacadeActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSCustomFacadeActivatorOptions>>().Value);
            services.TryAddSingleton<IJSCustomFacadeActivator, JSCustomFacadeActivator>();
            return services;
        }

        public static IServiceCollection AddJSFacadesActivator(this IServiceCollection services, Action<JSFacadesActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            ComponentPropertyAssignersUtils.ForEachDefaultComponentPropertyAssigner(defaultComponentPropertyAssignmentType => {
                services.TryAddSingleton<ICollectibleComponentPropertyAssigner>(serviceProvider =>
                    new CollectibleComponentPropertyAssigner(defaultComponentPropertyAssignmentType));
            });

            services.AddSingleton<IPostConfigureOptions<JSFacadesActivatorOptions>, JSFacadesActivatorOptionsPostConfiguration>();
            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSFacadesActivatorOptions>>().Value);
            services.TryAddSingleton<IJSFacadesActivator, JSFacadesActivator>();
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSFacades(this IServiceCollection services)
        {
            services.AddJSLocalObject();
            services.AddJSModule();
            AddJSCustomFacadeActivator(services);
            services.AddJSFacadesActivator();
            return services;
        }
    }
}
