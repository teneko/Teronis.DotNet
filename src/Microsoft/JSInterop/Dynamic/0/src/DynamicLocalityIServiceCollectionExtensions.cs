// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.ServiceBuilder;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicLocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorServicesOptionsIServiceCollectionExtensions.AddDynamicInterceptorServicesOptions{TInterceptorServicesOptions, TAssignerServicesOptions}(IServiceCollection)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxyActivator(IServiceCollection, System.Action{Dynamic.JSDynamicProxyValueAssignerServicesOptions}?, System.Action{Dynamic.JSDynamicProxyInterceptorServicesOptions}?, Teronis.Microsoft.JSInterop.Interception.ServiceBuilder.PostConfigureInterceptorServicesOptionsDelegate{Dynamic.JSDynamicProxyInterceptorServicesOptions, Dynamic.JSDynamicProxyValueAssignerServicesOptions}?)"/>,
        /// <see cref="LocalityIServiceCollectionExtensions.AddJSLocalObjectActivator(IServiceCollection, System.Action{JSLocalObjectValueAssignerServicesOptions}?, System.Action{JSLocalObjectInterceptorServicesOptions}?, Teronis.Microsoft.JSInterop.Interception.ServiceBuilder.PostConfigureInterceptorServicesOptionsDelegate{JSLocalObjectInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions}?)"/>
        /// and tries to add <see cref="JSDynamicLocalObjectActivator"/> as <see cref="IJSDynamicLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicLocalObjectActivator(this IServiceCollection services)
        {
            services.AddDynamicInterceptorServicesOptions<JSLocalObjectInterceptorServicesOptions, JSLocalObjectValueAssignerServicesOptions>();
            services.AddJSDynamicProxyActivator();
            services.AddJSLocalObjectActivator();
            services.TryAddSingleton<IJSDynamicLocalObjectActivator, JSDynamicLocalObjectActivator>();
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
