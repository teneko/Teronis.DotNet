// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;
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
        /// <param name="configureValueAssignerOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(
            this IServiceCollection services, 
            Action<JSLocalObjectValueAssignerOptions>? configureValueAssignerOptions = null,
            Action<JSLocalObjectInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            ConfigureMutableInterceptorBuilderDelegate<JSLocalObjectInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            services.AddInterceptorBuilderOptions<JSLocalObjectInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configureValueAssignerOptions, configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectValueAssignerOptions}?, Action{JSLocalObjectInterceptorBuilderOptions}?, ConfigureMutableInterceptorBuilderDelegate{JSLocalObjectInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions}?)"/>.
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
