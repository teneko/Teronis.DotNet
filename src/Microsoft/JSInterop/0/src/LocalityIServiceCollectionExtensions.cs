// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Locality.WebAssets;

namespace Teronis.Microsoft.JSInterop
{
    public static class LocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to configure options for <see cref="JSLocalObjectActivatorOptions"/> and 
        /// tries to add <see cref="JSLocalObjectActivatorOptions"/> as singleton.
        /// Tries to adds <see cref="JSLocalObjectActivator"/> as <see cref="IJSLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectActivator(this IServiceCollection services, Action<JSLocalObjectActivatorOptions>? configureOptions = null)
        {
            services.TryAddSingleton<IConfigureOptions<JSLocalObjectActivatorOptions>>(serviceProvider =>
                JSFunctionalObjectOptionsConfiguration<JSLocalObjectActivatorOptions>.Create(serviceProvider));

            services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<JSLocalObjectActivatorOptions>>().Value);

            if (!(configureOptions is null)) {
                services.Configure(configureOptions);
            }

            services.AddTransient<IJSLocalObjectActivator, JSLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="WebAssets.IServiceCollectionExtensions.AddJSLocalObjectInterop(IServiceCollection)"/>>
        /// and <see cref="AddJSLocalObjectActivator(IServiceCollection, Action{JSLocalObjectActivatorOptions}?)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
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
