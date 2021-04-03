// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection.Extensions
{
    public static class IScopedServiceCollectionExtensions
    {
        private readonly static ILifetimeServiceCollectionExtensionsTemplate<ScopedServiceDescriptor, IScopedServiceCollection> extensionsTemplate =
            new ILifetimeServiceCollectionExtensionsTemplate<ScopedServiceDescriptor, IScopedServiceCollection>(ScopedServiceDescriptorActivator.Instance);

        public static IScopedServiceCollection Add(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            extensionsTemplate.Add(collection, descriptor);

        public static IScopedServiceCollection Add(this IScopedServiceCollection collection, IEnumerable<ScopedServiceDescriptor> descriptors) =>
            extensionsTemplate.Add(collection, descriptors);

        public static IScopedServiceCollection RemoveAll(this IScopedServiceCollection collection, Type serviceType) =>
            extensionsTemplate.RemoveAll(collection, serviceType);

        public static IScopedServiceCollection RemoveAll<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            extensionsTemplate.RemoveAll<TService>(collection);

        public static IScopedServiceCollection Replace(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            extensionsTemplate.Replace(collection, descriptor);

        public static bool Contains(this IScopedServiceCollection collection, Type serviceType) =>
            extensionsTemplate.Contains(collection, serviceType);

        public static bool Contains<TService>(this IScopedServiceCollection collection) =>
            extensionsTemplate.Contains<TService>(collection);

        public static void TryAdd(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            extensionsTemplate.TryAdd(collection, descriptor);

        public static void TryAdd(this IScopedServiceCollection collection, IEnumerable<ScopedServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            extensionsTemplate.TryAddEnumerable(collection, descriptor);

        /// <summary>
        /// Adds <paramref name="descriptors"/> if not a single descriptor with same
        /// service type and same implementation does exist in <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptors"></param>
        /// <remarks>
        /// Prevents the registration of implementation type duplicates.
        /// </remarks>
        public static void TryAddEnumerable(this IScopedServiceCollection collection, IEnumerable<ScopedServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAddEnumerable(collection, descriptors);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service, Type implementationType) =>
            extensionsTemplate.TryAddService(collection, service, implementationType);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service) =>
            extensionsTemplate.TryAddService(collection, service);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            extensionsTemplate.TryAddService(collection, service, implementationFactory);

        public static void TryAddScoped<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            TryAddScoped(collection, typeof(TService));

        public static void TryAddScoped<TService, TImplementation>(this IScopedServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            extensionsTemplate.TryAddService<TService, TImplementation>(collection);

        public static void TryAddScoped<TService>(this IScopedServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            extensionsTemplate.TryAddService(collection, implementationFactory);

        public static IServiceCollectionAdapter<IScopedServiceCollection> CreateServiceCollectionAdapter(IScopedServiceCollection collection) =>
            extensionsTemplate.CreateServiceCollectionAdapter(collection);

        private class ScopedServiceDescriptorActivator : DescriptorActivator<ScopedServiceDescriptor>
        {
            public readonly static ScopedServiceDescriptorActivator Instance = new ScopedServiceDescriptorActivator();

            internal protected override ScopedServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new ScopedServiceDescriptor(serviceDescriptor);

            internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new ScopedServiceDescriptor(serviceType, implementationType);

            internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                throw new NotSupportedException("Cannot describe scoped service by instance");

            internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
                new ScopedServiceDescriptor(serviceType, implementationFactory);
        }
    }
}
