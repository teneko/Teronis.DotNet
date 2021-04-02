// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class CustomFacadeIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSFacadeHubActivatorInterceptorBuilderOptions"/>.
        /// Tries to add <see cref="JSCustomFacadeActivator"/> as <see cref="IJSCustomFacadeActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configureValueAssignerOptions"></param>
        /// <param name="configureInterceptorBuilderOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSCustomFacadeActivator(
            this IServiceCollection services,
            Action<JSCustomFacadeActivatorOptions>? configureOptions = null,
            Action<JSFacadeHubActivatorValueAssignerOptions>? configureValueAssignerOptions = null,
            Action<JSFacadeHubActivatorInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            ConfigureMutableInterceptorBuilderDelegate<JSFacadeHubActivatorInterceptorBuilderOptions, JSFacadeHubActivatorValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddInterceptorBuilderOptions<JSFacadeHubActivatorInterceptorBuilderOptions, JSFacadeHubActivatorValueAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configureValueAssignerOptions, configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSCustomFacadeActivator, JSCustomFacadeActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSCustomFacadeActivator(IServiceCollection, Action{JSCustomFacadeActivatorOptions}?, Action{JSFacadeHubActivatorValueAssignerOptions}?, Action{JSFacadeHubActivatorInterceptorBuilderOptions}?, ConfigureMutableInterceptorBuilderDelegate{JSFacadeHubActivatorInterceptorBuilderOptions, JSFacadeHubActivatorValueAssignerOptions}?)"/>.
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
