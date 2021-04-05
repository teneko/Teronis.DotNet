// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    internal readonly struct StaticLifetimeServicesExtension<TServiceBase, TProvider, TDescriptor, TCollection>
        where TServiceBase : class
        where TProvider : class, IServiceProvider
        where TDescriptor : LifetimeServiceDescriptor<TProvider>
        where TCollection : ILifetimeServiceCollection<TDescriptor>
    {
        internal readonly DescriptorActivatorBase<TProvider, TDescriptor> DescriptorActivator;

        public StaticLifetimeServicesExtension(DescriptorActivatorBase<TProvider, TDescriptor> descriptorActivator) =>
            this.DescriptorActivator = descriptorActivator ?? throw new ArgumentNullException(nameof(descriptorActivator));

        public TCollection Add(TCollection collection, TDescriptor descriptor)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            collection.Add(descriptor);
            return collection;
        }

        public TCollection Add(TCollection collection, IEnumerable<TDescriptor> descriptors)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptors is null) {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var descriptor in descriptors) {
                collection.Add(descriptor);
            }

            return collection;
        }

        public TCollection RemoveAll(TCollection collection, Type serviceType)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (serviceType is null) {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var collectionCount = collection.Count;

            for (var index = collectionCount - 1; index >= 0; index--) {
                if (collection[index].ServiceType != serviceType) {
                    continue;
                }

                collection.RemoveAt(index);
            }

            return collection;
        }

        public TCollection RemoveAll<TService>(TCollection collection)
            where TService : class, TServiceBase
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            return RemoveAll(collection, typeof(TService));
        }

        public TCollection Replace(TCollection collection, TDescriptor descriptor)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var collectionCount = collection.Count;
            var descriptorIndex = -1;

            for (var currentDescriptorIndex = 0; currentDescriptorIndex < collectionCount; currentDescriptorIndex++) {
                if (collection[currentDescriptorIndex].ServiceType != descriptor.ServiceType) {
                    continue;
                }

                descriptorIndex = currentDescriptorIndex;
                break;
            }

            if (descriptorIndex != -1) {
                collection[descriptorIndex] = descriptor;
            }

            return collection;
        }

        public bool Contains(TCollection collection, Type serviceType)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (serviceType is null) {
                throw new ArgumentNullException(nameof(serviceType));
            }

            return collection.Any(descriptor => descriptor.ServiceType == serviceType);
        }

        public bool Contains<TService>(TCollection collection)
            where TService : class, TServiceBase =>
            Contains(collection, typeof(TService));

        public void TryAdd(TCollection collection, TDescriptor descriptor)
        {
            if (Contains(collection, descriptor.ServiceType)) {
                return;
            }

            collection.Add(descriptor);
        }

        public void TryAdd(TCollection collection, IEnumerable<TDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors) {
                TryAdd(collection, descriptor);
            }
        }

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public void TryAddEnumerable(TCollection collection, TDescriptor descriptor)
        {
            if (collection is null) {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null) {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (collection.Any(collectionDescriptor => {
                return collectionDescriptor.ServiceType == descriptor.ServiceType
                    && collectionDescriptor.GetImplementationType() == descriptor.GetImplementationType();
            })) {
                return;
            }

            collection.Add(descriptor);
        }

        /// <summary>
        /// Adds <paramref name="descriptors"/> if not a single descriptor with same
        /// service type and same implementation does exist in <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptors"></param>
        /// <remarks>
        /// Prevents the registration of implementation type duplicates.
        /// </remarks>
        public void TryAddEnumerable(TCollection collection, IEnumerable<TDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors) {
                TryAddEnumerable(collection, descriptor);
            }
        }

        public void TryAddService(TCollection collection, Type service, Type implementationType)
        {
            if (Contains(collection, service)) {
                return;
            }

            if (implementationType is null) {
                throw new ArgumentNullException(nameof(implementationType));
            }

            collection.Add(DescriptorActivator.CreateDescriptor(service, implementationType));
        }

        public void TryAddService(TCollection collection, Type service) =>
            TryAddService(collection, service, service);

        public void TryAddService(TCollection collection, Type service, ImplementationFactoryDelegate<TProvider, object> implementationFactory)
        {
            if (Contains(collection, service)) {
                return;
            }

            if (implementationFactory is null) {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            collection.Add(DescriptorActivator.CreateDescriptor(service, implementationFactory));
        }

        public void TryAddService<TService>(TCollection collection)
            where TService : class, TServiceBase =>
            TryAddService(collection, typeof(TService));

        public void TryAddService<TService, TImplementation>(TCollection collection)
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            TryAddService(collection, typeof(TService), typeof(TImplementation));

        public void TryAddService<TService>(TCollection collection, TService instance)
            where TService : class, TServiceBase
        {
            if (instance is null) {
                throw new ArgumentNullException(nameof(instance));
            }

            var serviceType = typeof(TService);

            if (Contains(collection, serviceType)) {
                return;
            }

            collection.Add(DescriptorActivator.CreateDescriptor(serviceType, instance));
        }

        public void TryAddService<TService>(TCollection collection, ImplementationFactoryDelegate<TProvider, TService> implementationFactory)
            where TService : class, TServiceBase =>
            TryAddService(collection, typeof(TService), implementationFactory);

        /* BEGIN HELPERS */

        public TCollection AddService<TService, TImplementation>(TCollection collection, ImplementationFactoryDelegate<TProvider, TImplementation> implementationFactory)
            where TService : class, TServiceBase
            where TImplementation : class, TService
        {
            collection.Add(DescriptorActivator.CreateDescriptor(typeof(TService), implementationFactory));
            return collection;
        }

        public TCollection AddService<TService>(TCollection collection, ImplementationFactoryDelegate<TProvider, TService> implementationFactory)
            where TService : class, TServiceBase
        {
            collection.Add(DescriptorActivator.CreateDescriptor(typeof(TService), implementationFactory));
            return collection;
        }

        public TCollection AddService<TService>(TCollection collection)
            where TService : class, TServiceBase
        {
            collection.Add(DescriptorActivator.CreateDescriptor(typeof(TService), typeof(TService)));
            return collection;
        }

        public TCollection AddService(TCollection collection, Type serviceType)
        {
            collection.Add(DescriptorActivator.CreateDescriptor(serviceType, serviceType));
            return collection;
        }

        public TCollection AddService<TService, TImplementation>(TCollection collection)
            where TService : class, TServiceBase
            where TImplementation : class, TService
        {
            collection.Add(DescriptorActivator.CreateDescriptor(typeof(TService), typeof(TImplementation)));
            return collection;
        }

        public TCollection AddService(TCollection collection, Type serviceType, ImplementationFactoryDelegate<TProvider, object> implementationFactory)
        {
            collection.Add(DescriptorActivator.CreateDescriptor(serviceType, implementationFactory));
            return collection;
        }

        public TCollection AddService(TCollection collection, Type serviceType, Type implementationType)
        {
            collection.Add(DescriptorActivator.CreateDescriptor(serviceType, implementationType));
            return collection;
        }

        public TCollection AddService<TService>(TCollection collection, TService implementationInstance)
            where TService : class, TServiceBase
        {
            collection.Add(DescriptorActivator.CreateDescriptor(typeof(TService), implementationInstance));
            return collection;
        }

        public TCollection AddService(TCollection collection, Type serviceType, object implementationInstance)
        {
            collection.Add(DescriptorActivator.CreateDescriptor(serviceType, implementationInstance));
            return collection;
        }

        /* END HELPERS */
    }
}
