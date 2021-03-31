// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Facades.PropertyAssigners;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class FacadesIServiceCollectionExtensions
    {
        public static IServiceCollection AddJSCustomFacadeActivator(this IServiceCollection services, Action<JSCustomFacadeActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddSingleton<IJSCustomFacadeActivator, JSCustomFacadeActivator>();
            return services;
        }

        public static IServiceCollection AddJSFacadeHubActivator(this IServiceCollection services, Action<JSFacadeHubActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.ConfigureOptions<DefaultPropertyAssignersPostConfiguration>();
            services.ConfigureOptions<JSFacadeHubActivatorOptionsPostConfiguration>();
            services.TryAddSingleton<IJSFacadeHubActivator, JSFacadeHubActivator>();
            services.TryAddTransient(typeof(IJSFacadeHub<>), typeof(JSFacadeHubService<>));
            return services;
        }

        /// <summary>
        /// Calls <see cref="LocalityIServiceCollectionExtensions.AddJSLocalObject(IServiceCollection)"/>,
        /// <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="AddJSCustomFacadeActivator(IServiceCollection, Action{JSCustomFacadeActivatorOptions}?)"/>
        /// and <see cref="AddJSFacadeHubActivator(IServiceCollection, Action{JSFacadeHubActivatorOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSFacadeHub(this IServiceCollection services)
        {
            services.AddJSLocalObject();
            services.AddJSModule();
            AddJSCustomFacadeActivator(services);
            services.AddJSFacadeHubActivator();
            return services;
        }
    }
}
