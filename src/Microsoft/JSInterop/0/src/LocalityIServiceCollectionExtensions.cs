// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class LocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSLocalObjectInterceptorBuilderOptions"/>.
        /// Tries to add <see cref="JSLocalObjectActivator"/> as <see cref="IJSLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureInterceptorBuilderOptions"></param>
        /// <param name="configurePropertyAssignerOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(
            this IServiceCollection services, 
            Action<JSLocalObjectPropertyAssignerOptions>? configurePropertyAssignerOptions = null,
            Action<JSLocalObjectInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            LateConfigureInterceptorBuilderDelegate<JSLocalObjectInterceptorBuilderOptions, JSLocalObjectPropertyAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            services.AddInterceptorBuilderOptions<JSLocalObjectInterceptorBuilderOptions, JSLocalObjectPropertyAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configurePropertyAssignerOptions, configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectPropertyAssignerOptions}?, Action{JSLocalObjectInterceptorBuilderOptions}?, LateConfigureInterceptorBuilderDelegate{JSLocalObjectInterceptorBuilderOptions, JSLocalObjectPropertyAssignerOptions}?)"/>.
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
