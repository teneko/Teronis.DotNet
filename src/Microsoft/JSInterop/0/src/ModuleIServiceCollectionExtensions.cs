// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop
{
    public static class ModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureValueAssignerServicesOptions"></param>
        /// <param name="configureInterceptorServicesOptions"></param>
        /// <param name="postConfigureInterceptorServicesOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModuleActivator(
            this IServiceCollection services,
            Action<JSModuleValueAssignerServicesOptions>? configureValueAssignerServicesOptions = null,
            Action<JSModuleInterceptorServicesOptions>? configureInterceptorServicesOptions = null,
            PostConfigureInterceptorServicesOptionsDelegate<JSModuleInterceptorServicesOptions, JSModuleValueAssignerServicesOptions>? postConfigureInterceptorServicesOptions = null)
        {
            services.AddInterceptorServicesOptions<JSModuleInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions>();
            services.ConfigureInterceptorServicesOptions(configureValueAssignerServicesOptions,configureInterceptorServicesOptions, postConfigureInterceptorServicesOptions);
            services.TryAddSingleton<IJSModuleActivator, JSModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSModuleActivator(IServiceCollection, Action{JSModuleValueAssignerServicesOptions}?, Action{JSModuleInterceptorServicesOptions}?, PostConfigureInterceptorServicesOptionsDelegate{JSModuleInterceptorServicesOptions, JSModuleValueAssignerServicesOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModule(this IServiceCollection services)
        {
            services.AddJSModuleActivator();
            return services;
        }
    }
}
