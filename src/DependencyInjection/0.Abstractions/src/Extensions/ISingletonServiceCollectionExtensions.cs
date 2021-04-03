// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection.Extensions
{
    public static class ISingletonServiceCollectionExtensions
    {
        private readonly static ILifetimeServiceCollectionExtensionsTemplate<SingletonServiceDescriptor, ISingletonServiceCollection> extensionsTemplate =
            new ILifetimeServiceCollectionExtensionsTemplate<SingletonServiceDescriptor, ISingletonServiceCollection>(SingletonServiceDescriptorActivator.Instance);

        public static ISingletonServiceCollection Add(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            extensionsTemplate.Add(collection, descriptor);

        public static ISingletonServiceCollection Add(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            extensionsTemplate.Add(collection, descriptors);

        public static ISingletonServiceCollection RemoveAll(this ISingletonServiceCollection collection, Type serviceType) =>
            extensionsTemplate.RemoveAll(collection, serviceType);

        public static ISingletonServiceCollection RemoveAll<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            extensionsTemplate.RemoveAll<TService>(collection);

        public static ISingletonServiceCollection Replace(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            extensionsTemplate.Replace(collection, descriptor);

        public static bool Contains(this ISingletonServiceCollection collection, Type serviceType) =>
            extensionsTemplate.Contains(collection, serviceType);

        public static bool Contains<TService>(this ISingletonServiceCollection collection) =>
            extensionsTemplate.Contains<TService>(collection);

        public static void TryAdd(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            extensionsTemplate.TryAdd(collection, descriptor);

        public static void TryAdd(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
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
        public static void TryAddEnumerable(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAddEnumerable(collection, descriptors);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service, Type implementationType) =>
            extensionsTemplate.TryAddService(collection, service, implementationType);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service) =>
            extensionsTemplate.TryAddService(collection, service);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            extensionsTemplate.TryAddService(collection, service, implementationFactory);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            TryAddSingleton(collection, typeof(TService));

        public static void TryAddSingleton<TService, TImplementation>(this ISingletonServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            extensionsTemplate.TryAddService<TService, TImplementation>(collection);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection, TService instance)
            where TService : class =>
            extensionsTemplate.TryAddService(collection, instance);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            extensionsTemplate.TryAddService(collection, implementationFactory);

        public static IServiceCollectionAdapter<ISingletonServiceCollection> CreateServiceCollectionAdapter(ISingletonServiceCollection collection) =>
            extensionsTemplate.CreateServiceCollectionAdapter(collection);

        private class SingletonServiceDescriptorActivator : DescriptorActivator<SingletonServiceDescriptor>
        {
            public readonly static SingletonServiceDescriptorActivator Instance = new SingletonServiceDescriptorActivator();

            internal protected override SingletonServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new SingletonServiceDescriptor(serviceDescriptor);

            internal protected override SingletonServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new SingletonServiceDescriptor(serviceType, implementationType);

            internal protected override SingletonServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                new SingletonServiceDescriptor(serviceType, implementationInstance);

            internal protected override SingletonServiceDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
                new SingletonServiceDescriptor(serviceType, implementationFactory);
        }
    }
}
