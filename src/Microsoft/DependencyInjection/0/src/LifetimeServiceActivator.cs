// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class LifetimeServiceActivator
    {
        /// <summary>
        /// If <paramref name="descriptor"/> has instance it gets returned.
        /// If <paramref name="descriptor"/> has factory the result gets returned.
        /// Otherwise the instance will be created by using the implemenetation type.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that is going to be passed to factory or used for
        /// instance creation by implementation type and <see cref="ActivatorUtilities"/>.
        /// </param>
        /// <param name="descriptor">Describes the instance.</param>
        /// <returns></returns>
        public static object GetInstanceOrCreateInstance(IServiceProvider serviceProvider, LifetimeServiceDescriptor descriptor)
        {
            if (!(descriptor.ImplementationInstance is null)) {
                return descriptor.ImplementationInstance;
            }

            if (!(descriptor.ImplementationFactory is null)) {
                return descriptor.ImplementationFactory(serviceProvider);
            }

            return ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ImplementationType!);
        }

        public static object GetServiceOrInstanceOrCreateInstance(IServiceProvider serviceProvider, LifetimeServiceDescriptor descriptor) {
            var service = serviceProvider.GetService(descriptor.ServiceType);

            if (!(service is null)) {
                return service;
            }

            if (!(descriptor.ImplementationInstance is null)) {
                return descriptor.ImplementationInstance;
            }

            if (!(descriptor.ImplementationFactory is null)) {
                return descriptor.ImplementationFactory(serviceProvider);
            }

            return ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ImplementationType!);
        }

        public static object GetInstanceOrServiceOrCreateInstance(IServiceProvider serviceProvider, LifetimeServiceDescriptor descriptor)
        {
            if (!(descriptor.ImplementationInstance is null)) {
                return descriptor.ImplementationInstance;
            }

            var service = serviceProvider.GetService(descriptor.ServiceType);

            if (!(service is null)) {
                return service;
            }

            if (!(descriptor.ImplementationFactory is null)) {
                return descriptor.ImplementationFactory(serviceProvider);
            }

            return ActivatorUtilities.CreateInstance(serviceProvider, descriptor.ImplementationType!);
        }
    }
}
