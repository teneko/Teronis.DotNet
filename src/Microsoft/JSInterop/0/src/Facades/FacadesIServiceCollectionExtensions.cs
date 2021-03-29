// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Facades.PropertyAssigners;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

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

        public static IServiceCollection AddJSFacadesActivator(this IServiceCollection services, Action<JSFacadesActivatorOptions>? configureOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddSingleton<IPostConfigureOptions<JSFacadesActivatorOptions>, DefaultPropertyAssignersPostConfiguration>();
            services.AddSingleton<IPostConfigureOptions<JSFacadesActivatorOptions>, JSFacadesActivatorOptionsPostConfiguration>();
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
