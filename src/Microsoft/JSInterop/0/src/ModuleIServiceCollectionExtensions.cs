// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop
{
    public static class ModuleIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSModuleActivator"/> as <see cref="IJSModuleActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSModuleActivator(this IServiceCollection services, Action<JSModuleActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSModuleActivatorOptions>>(serviceProvider =>
                JSObjectInterceptorBuilderOptionsConfiguration<JSModuleActivatorOptions>.Create(serviceProvider));

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddTransient<IJSModuleActivator, JSModuleActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSModuleActivator(IServiceCollection, Action{JSModuleActivatorOptions}?)"/>.
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
