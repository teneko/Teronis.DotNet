// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class CustomFacadeIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSFacadeHubActivatorInterceptorServicesOptions"/>.
        /// Tries to add <see cref="JSCustomFacadeActivator"/> as <see cref="IJSCustomFacadeActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configureValueAssignerServicesOptions"></param>
        /// <param name="configureInterceptorServicesOptions"></param>
        /// <param name="postConfigureInterceptorServicesOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSCustomFacadeActivator(
            this IServiceCollection services,
            Action<JSCustomFacadeActivatorOptions>? configureOptions = null,
            Action<JSFacadeHubActivatorValueAssignerServicesOptions>? configureValueAssignerServicesOptions = null,
            Action<JSFacadeHubActivatorInterceptorServicesOptions>? configureInterceptorServicesOptions = null,
            PostConfigureInterceptorServicesOptionsDelegate<JSFacadeHubActivatorInterceptorServicesOptions, JSFacadeHubActivatorValueAssignerServicesOptions>? postConfigureInterceptorServicesOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.ConfigureInterceptorServicesOptions(configureValueAssignerServicesOptions, configureInterceptorServicesOptions, postConfigureInterceptorServicesOptions);
            services.TryAddSingleton<IJSCustomFacadeActivator, JSCustomFacadeActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSCustomFacadeActivator(IServiceCollection, Action{JSCustomFacadeActivatorOptions}?, Action{JSFacadeHubActivatorValueAssignerServicesOptions}?, Action{JSFacadeHubActivatorInterceptorServicesOptions}?, PostConfigureInterceptorServicesOptionsDelegate{JSFacadeHubActivatorInterceptorServicesOptions, JSFacadeHubActivatorValueAssignerServicesOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSCustomFacade(this IServiceCollection services)
        {
            services.AddJSModule();
            services.AddJSLocalObjectInterop();
            services.AddJSCustomFacadeActivator();
            return services;
        }
    }
}
