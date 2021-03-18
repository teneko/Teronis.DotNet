using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSFacadeResolver(this IServiceCollection services, Action<JSFacadeResolverOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSFacadeResolverOptions>>().Value);
            services.TryAddSingleton<IJSFacadeResolver, JSFacadeResolver>();
            return services;
        }

        public static IServiceCollection AddJSFacadesActivator(this IServiceCollection services, Action<JSFacadesActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddSingleton<IPostConfigureOptions<JSFacadesActivatorOptions>>(sp => new PostConfigureOptions<JSFacadesActivatorOptions>(
                name: string.Empty,
                options => {
                    foreach (var componentPropertyAssignmentFactory in options.ComponentPropertyAssignmentFactories.Values) {
                        var componentPropertyAssignment = componentPropertyAssignmentFactory(sp);
                        options.ComponentPropertyAssignments.Add(componentPropertyAssignment);
                    }
                }));

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
            AddJSFacadeResolver(services);
            services.AddJSFacadesActivator();
            return services;
        }
    }
}
