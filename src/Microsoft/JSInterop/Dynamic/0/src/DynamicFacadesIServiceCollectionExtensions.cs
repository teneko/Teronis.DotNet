// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.JSInterop.Facade;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicFacadesIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicIValueAssignerServicesOptionsIServiceCollectionExtensions.AddDynamicValueAssignerServicesOptions{TAssignerServicesOptions}(IServiceCollection)"/>,
        /// <see cref="DynamicLocalityIServiceCollectionExtensions.AddJSDynamicLocalObject(IServiceCollection)"/>,
        /// <see cref="DynamicModuleIServiceCollectionExtensions.AddJSDynamicModule(IServiceCollection)"/>,
        /// and <see cref="FacadesIServiceCollectionExtensions.AddJSFacadeHub(IServiceCollection)"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicFacadeHub(this IServiceCollection services)
        {
            services.AddDynamicValueAssignerServicesOptions<JSFacadeHubActivatorValueAssignerServicesOptions>();
            services.AddJSDynamicLocalObject();
            services.AddJSDynamicModule();
            services.AddJSFacadeHub();
            return services;
        }
    }
}
