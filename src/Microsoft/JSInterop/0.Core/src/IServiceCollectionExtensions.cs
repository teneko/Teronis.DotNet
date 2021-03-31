// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop
{
    public static class IServiceCollectionExtensions
    {
        internal static IServiceCollection TryAddTypeUniqueSingleton<TService, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TService
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);

            if (!services.Any(serviceDescriptorComparand => ServiceDescriptorOnlyTypesEqualityComparer.Default.Equals(serviceDescriptor, serviceDescriptorComparand))) {
                services.Add(serviceDescriptor);
            }

            return services;
        }
    }
}
