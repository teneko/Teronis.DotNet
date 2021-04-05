// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Facade;

namespace Teronis.Microsoft.JSInterop
{
    public static class FacadesIServiceCollectionExtensions
    {
        public static IServiceCollection AddJSFacadeHubActivator(
            this IServiceCollection services,
            Action<JSFacadeHubActivatorOptions>? configureOptions = null,
             Action<JSFacadeHubActivatorValueAssignerServicesOptions>? configureValueAssigner = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddTypeUniqueSingleton<IConfigureOptions<JSFacadeHubActivatorOptions>, JSFacadeHubActivatorOptionsPostConfiguration>();
            services.AddValueAssignerServicesOptions<JSFacadeHubActivatorValueAssignerServicesOptions>();
            services.ConfigureValueAssignerServicesOptions(configureValueAssigner);
            services.TryAddSingleton<IJSFacadeHubActivator, JSFacadeHubActivator>();
            services.TryAddTransient(typeof(IJSFacadeHub<>), typeof(JSFacadeHubService<>));
            return services;
        }

        /// <summary>
        /// Calls <see cref="CustomFacadeIServiceCollectionExtensions.AddJSCustomFacade(IServiceCollection)"/>,
        /// <see cref="LocalityIServiceCollectionExtensions.AddJSLocalObject(IServiceCollection)"/>,
        /// <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// and <see cref="AddJSFacadeHubActivator(IServiceCollection, Action{JSFacadeHubActivatorOptions}?, Action{JSFacadeHubActivatorValueAssignerServicesOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSFacadeHub(this IServiceCollection services)
        {
            services.AddJSCustomFacade();
            services.AddJSLocalObject();
            services.AddJSModule();
            services.AddJSFacadeHubActivator();
            return services;
        }
    }
}
