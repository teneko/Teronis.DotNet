// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class LocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSLocalObjectInterceptorBuilderOptions"/>.
        /// Tries to add transient <see cref="JSLocalObjectActivator"/> as <see cref="IJSLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(this IServiceCollection services, Action<JSLocalObjectInterceptorBuilderOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSLocalObjectInterceptorBuilderOptions>>(serviceProvider =>
                JSObjectInterceptorBuilderOptionsConfiguration<JSLocalObjectInterceptorBuilderOptions>.Create(serviceProvider));

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.TryAddTransient<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="ModuleIServiceCollectionExtensions.AddJSModule(IServiceCollection)"/>,
        /// <see cref="LocalityWebAssetsIServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectInterceptorBuilderOptions}?)"/>.
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
