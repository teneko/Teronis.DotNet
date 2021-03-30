// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

        public static IServiceCollection AddJSFacadesActivator(this IServiceCollection services, Action<JSFacadeHubActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddSingleton<IPostConfigureOptions<JSFacadeHubActivatorOptions>, DefaultPropertyAssignersPostConfiguration>();
            services.AddSingleton<IPostConfigureOptions<JSFacadeHubActivatorOptions>, JSFacadesActivatorOptionsPostConfiguration>();
            services.TryAddSingleton<IJSFacadeHubActivator, JSFacadeHubActivator>();
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
