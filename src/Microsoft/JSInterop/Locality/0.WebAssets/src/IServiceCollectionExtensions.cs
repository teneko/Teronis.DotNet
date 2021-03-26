// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Locality.WebAssets
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Tries to add <see cref="JSLocalObjectInterop"/> as <see cref="IJSLocalObjectInterop"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJSLocalObjectInterop(this IServiceCollection services)
        {
            services.TryAddSingleton<IJSLocalObjectInterop, JSLocalObjectInterop>();
            return services;
        }
    }
}
