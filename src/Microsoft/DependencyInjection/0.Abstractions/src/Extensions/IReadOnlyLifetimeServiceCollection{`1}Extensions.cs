// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static class IReadOnlyLifetimeServiceCollectionExtensions
    {
        public static ServiceDescriptor CreateServiceDescriptor(this IReadOnlyLifetimeServiceCollection<LifetimeServiceDescriptor<IServiceProvider>> collection, LifetimeServiceDescriptor<IServiceProvider> descriptor)
        {
            if (!(descriptor.ImplementationInstance is null)) {
                return new ServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationInstance);
            }

            if (!(descriptor.ImplementationFactory is null)) {
                return new ServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationFactory.Invoke, collection.Lifetime);
            }

            return new ServiceDescriptor(descriptor.ServiceType, descriptor.ImplementationType!);
        }
    }
}
