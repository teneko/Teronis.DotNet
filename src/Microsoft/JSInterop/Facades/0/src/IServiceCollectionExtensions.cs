using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Locality.Dynamic;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSFacades(this IServiceCollection services)
        {
            services.AddJSDynamicLocalObjects();

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(IJSFacadeDictionary),
                    serviceProvider => new JSFacadeDictionaryBuilder()
                        .AddDefault()
                        .Build(),
                    ServiceLifetime.Singleton));

            services.TryAddSingleton<IJSFacadeResolver, JSFacadeResolver>();
            services.TryAddSingleton<IJSFunctionalFacadesActivator, JSFunctionalFacadesActivator>();
            services.TryAddSingleton<IJSFacadesActivator, JSFacadesActivator>();
            return services;
        }

        public static IServiceCollection AddJSFacadeDictionary(this IServiceCollection services, Action<IJSFacadeDictionaryBuilder> configureDictionaryBuilder)
        {
            var dictionaryBuilder = new JSFacadeDictionaryBuilder();
            configureDictionaryBuilder?.Invoke(dictionaryBuilder);

            return services.Replace(
                new ServiceDescriptor(
                    typeof(IJSFacadeDictionary),
                    serviceProvider => dictionaryBuilder.Build(),
                ServiceLifetime.Singleton));
        }
    }
}
