// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Dynamic.Facades.PropertyAssigners;
using Teronis.Microsoft.JSInterop.Facades;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class DynamicFacadesIServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicFacadeHubActivator(IServiceCollection services)
        {
            services.AddJSFacadeHubActivator();
            services.ConfigureOptions<DefaultPropertyAssignersPostConfiguration>();
            return services;
        }

        /// <summary>
        /// Calls <see cref="AddJSDynamicFacadeHubActivator(IServiceCollection)"/>
        /// <see cref="DynamicLocalityIServiceCollectionExtensions.AddJSDynamicLocalObject(IServiceCollection)"/>,
        /// <see cref="DynamicModuleIServiceCollectionExtensions.AddJSDynamicModule(IServiceCollection)"/>
        /// and <see cref="FacadesIServiceCollectionExtensions.AddJSFacadeHub(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicFacadeHub(this IServiceCollection services)
        {
            AddJSDynamicFacadeHubActivator(services);
            services.AddJSDynamicLocalObject();
            services.AddJSDynamicModule();
            services.AddJSFacadeHub();
            return services;
        }
    }
}
