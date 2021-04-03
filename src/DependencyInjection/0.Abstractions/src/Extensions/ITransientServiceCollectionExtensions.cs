// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection.Extensions
{
    public static class ITransientServiceCollectionExtensions
    {
        private readonly static ILifetimeServiceCollectionExtensionsTemplate<TransientServiceDescriptor, ITransientServiceCollection> extensionsTemplate =
            new ILifetimeServiceCollectionExtensionsTemplate<TransientServiceDescriptor, ITransientServiceCollection>(TransientServiceDescriptorActivator.Instance);

        public static ITransientServiceCollection Add(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            extensionsTemplate.Add(collection, descriptor);

        public static ITransientServiceCollection Add(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            extensionsTemplate.Add(collection, descriptors);

        public static ITransientServiceCollection RemoveAll(this ITransientServiceCollection collection, Type serviceType) =>
            extensionsTemplate.RemoveAll(collection, serviceType);

        public static ITransientServiceCollection RemoveAll<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            extensionsTemplate.RemoveAll<TService>(collection);

        public static ITransientServiceCollection Replace(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            extensionsTemplate.Replace(collection, descriptor);

        public static bool Contains(this ITransientServiceCollection collection, Type serviceType) =>
            extensionsTemplate.Contains(collection, serviceType);

        public static bool Contains<TService>(this ITransientServiceCollection collection) =>
            extensionsTemplate.Contains<TService>(collection);

        public static void TryAdd(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            extensionsTemplate.TryAdd(collection, descriptor);

        public static void TryAdd(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
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
        public static void TryAddEnumerable(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            extensionsTemplate.TryAddEnumerable(collection, descriptors);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service, Type implementationType) =>
            extensionsTemplate.TryAddService(collection, service, implementationType);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service) =>
            extensionsTemplate.TryAddService(collection, service);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            extensionsTemplate.TryAddService(collection, service, implementationFactory);

        public static void TryAddTransient<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            TryAddTransient(collection, typeof(TService));

        public static void TryAddTransient<TService, TImplementation>(this ITransientServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            extensionsTemplate.TryAddService<TService, TImplementation>(collection);

        public static void TryAddTransient<TService>(this ITransientServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            extensionsTemplate.TryAddService(collection, implementationFactory);

        public static IServiceCollectionAdapter<ITransientServiceCollection> CreateServiceCollectionAdapter(ITransientServiceCollection collection) =>
            extensionsTemplate.CreateServiceCollectionAdapter(collection);

        private class TransientServiceDescriptorActivator : DescriptorActivator<TransientServiceDescriptor>
        {
            public readonly static TransientServiceDescriptorActivator Instance = new TransientServiceDescriptorActivator();

            internal protected override TransientServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new TransientServiceDescriptor(serviceDescriptor);

            internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new TransientServiceDescriptor(serviceType, implementationType);

            internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                throw new NotSupportedException("Cannot describe transient service by instance");

            internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
                new TransientServiceDescriptor(serviceType, implementationFactory);
        }
    }
}
