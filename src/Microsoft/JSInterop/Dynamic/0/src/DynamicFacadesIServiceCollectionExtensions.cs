// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Dynamic.Facades.PropertyAssigners;
using Teronis.Microsoft.JSInterop.Facades;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class DynamicFacadesIServiceCollectionExtensions
    {
        public static IServiceCollection AddJSDynamicFacadesActivator(IServiceCollection services)
        {
            services.AddJSFacadesActivator();
            services.AddSingleton<IPostConfigureOptions<JSFacadeHubActivatorOptions>, DefaultPropertyAssignersPostConfiguration>();
            return services;
        }

        public static IServiceCollection AddJSDynamicFacades(this IServiceCollection services)
        {
            AddJSDynamicFacadesActivator(services);
            services.AddJSFacades();
            return services;
        }
    }
}
