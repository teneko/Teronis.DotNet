// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Dynamic.Activators;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class DynamicLocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add transient <see cref="JSDynamicLocalObjectActivator"/> as <see cref="IJSDynamicLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicLocalObjectActivator(this IServiceCollection services)
        {
            services.TryAddTransient<IJSDynamicLocalObjectActivator, JSDynamicLocalObjectActivator>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="LocalityIServiceCollectionExtensions.AddJSLocalObject(IServiceCollection)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxy(IServiceCollection)"/>
        /// and <see cref="AddJSDynamicLocalObjectActivator(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicLocalObject(this IServiceCollection services)
        {

            services.AddJSLocalObject();
            services.AddJSDynamicProxy();
            services.AddJSDynamicLocalObjectActivator();
            return services;
        }
    }
}
