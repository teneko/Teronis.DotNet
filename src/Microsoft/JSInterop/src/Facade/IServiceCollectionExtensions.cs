using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSFacade(this IServiceCollection services)
        {
            services.AddSingleton<IJSFacadeDictionary>(serviceProvider => new JSFacadeDictionaryBuilder()
                .AddDefault()
                .Build());

            services.TryAdd(
                new ServiceDescriptor(
                    typeof(IJSFacadeDictionary),
                    serviceProvider => new JSFacadeDictionaryBuilder()
                        .AddDefault()
                        .Build(),
                    ServiceLifetime.Singleton));

            services.AddSingleton<IJSFacadeResolver, JSFacadeResolver>();
            services.AddSingleton<IIndependentJSFacadePropertiesInitializer, IndependentJSFacadePropertiesInitializer>();
            services.AddSingleton<IJSFacadePropertiesInitializer, JSFacadePropertiesInitializer>();
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
