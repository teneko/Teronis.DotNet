using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Facade.WebAssets;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJSFacade(this IServiceCollection services)
        {
            services.TryAdd(
                new ServiceDescriptor(
                    typeof(IJSFacadeDictionary),
                    serviceProvider => new JSFacadeDictionaryBuilder()
                        .AddDefault()
                        .Build(),
                    ServiceLifetime.Singleton));

            services.AddSingleton<IJSFacadeResolver, JSFacadeResolver>();
            services.AddSingleton<IJSIndependentFacadesInitializer, JSIndependentFacadesInitializer>();
            services.AddSingleton<IJSFacadesInitializer, JSFacadesInitializer>();
            services.AddSingleton<IJSObjectInterop, JSObjectInterop>();
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
