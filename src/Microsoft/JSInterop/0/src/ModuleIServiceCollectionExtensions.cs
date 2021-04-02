// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;
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
        /// <param name="configureValueAssignerOptions"></param>
        /// <param name="configureInterceptorBuilderOptions"></param>
        /// <param name="lateConfigureInterceptorBuilderOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModuleActivator(
            this IServiceCollection services,
            Action<JSModuleValueAssignerOptions>? configureValueAssignerOptions = null,
            Action<JSModuleInterceptorBuilderOptions>? configureInterceptorBuilderOptions = null,
            ConfigureMutableInterceptorBuilderDelegate<JSModuleInterceptorBuilderOptions, JSModuleValueAssignerOptions>? lateConfigureInterceptorBuilderOptions = null)
        {
            services.AddInterceptorBuilderOptions<JSModuleInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions>();
            services.ConfigureInterceptorBuilderOptions(configureValueAssignerOptions,configureInterceptorBuilderOptions, lateConfigureInterceptorBuilderOptions);
            services.TryAddSingleton<IJSModuleActivator, JSModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSModuleActivator(IServiceCollection, Action{JSModuleValueAssignerOptions}?, Action{JSModuleInterceptorBuilderOptions}?, ConfigureMutableInterceptorBuilderDelegate{JSModuleInterceptorBuilderOptions, JSModuleValueAssignerOptions}?)"/>.
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
