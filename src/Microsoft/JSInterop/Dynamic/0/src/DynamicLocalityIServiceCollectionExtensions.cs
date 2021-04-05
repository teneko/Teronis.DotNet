// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception;
using Teronis.Microsoft.JSInterop.Locality;

namespace Teronis.Microsoft.JSInterop
{
    public static class DynamicLocalityIServiceCollectionExtensions
    {
        /// <summary>
        /// Calls <see cref="DynamicJSInterceptorBuilderOptionsIServiceCollectionExtensions.AddDynamicInterceptorBuilderOptions{TDerivedBuilderOptions, TDerivedAssignerOptions}(IServiceCollection)"/>,
        /// <see cref="DynamicIServiceCollectionExtensions.AddJSDynamicProxyActivator(IServiceCollection, System.Action{Dynamic.JSDynamicProxyValueAssignerOptions}?, System.Action{Dynamic.JSDynamicProxyInterceptorBuilderOptions}?, Interception.PostConfigureInterceptorBuilderDelegate{Dynamic.JSDynamicProxyInterceptorBuilderOptions, Dynamic.JSDynamicProxyValueAssignerOptions}?)"/>,
        /// <see cref="LocalityIServiceCollectionExtensions.AddJSLocalObjectActivator(IServiceCollection, System.Action{JSLocalObjectValueAssignerOptions}?, System.Action{JSLocalObjectInterceptorBuilderOptions}?, Interception.PostConfigureInterceptorBuilderDelegate{JSLocalObjectInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions}?)"/>
        /// and tries to add <see cref="JSDynamicLocalObjectActivator"/> as <see cref="IJSDynamicLocalObjectActivator"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSDynamicLocalObjectActivator(this IServiceCollection services)
        {
            services.AddDynamicInterceptorBuilderOptions<JSLocalObjectInterceptorBuilderOptions, JSLocalObjectValueAssignerOptions>();
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
