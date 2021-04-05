// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class LocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSLocalObjectInterceptorServicesOptions"/>.
        /// Tries to add <see cref="JSLocalObjectActivator"/> as <see cref="IJSLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureInterceptorServicesOptions"></param>
        /// <param name="configureValueAssignerServicesOptions"></param>
        /// <param name="postConfigureInterceptorServicesOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(
            this IServiceCollection services, 
            Action<JSLocalObjectValueAssignerServicesOptions>? configureValueAssignerServicesOptions = null,
            Action<JSLocalObjectInterceptorServicesOptions>? configureInterceptorServicesOptions = null,
            PostConfigureInterceptorServicesOptionsDelegate<JSLocalObjectInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions>? postConfigureInterceptorServicesOptions = null)
        {
            services.AddInterceptorServicesOptions<JSLocalObjectInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions>();
            services.ConfigureInterceptorServicesOptions(configureValueAssignerServicesOptions, configureInterceptorServicesOptions, postConfigureInterceptorServicesOptions);
            services.TryAddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectValueAssignerServicesOptions}?, Action{JSLocalObjectInterceptorServicesOptions}?, PostConfigureInterceptorServicesOptionsDelegate{JSLocalObjectInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObject(this IServiceCollection services)
        {
            services.AddJSModule();
            services.AddJSLocalObjectInterop();
            services.AddJSLocalObjectActivator();
            return services;
        }
    }
}
