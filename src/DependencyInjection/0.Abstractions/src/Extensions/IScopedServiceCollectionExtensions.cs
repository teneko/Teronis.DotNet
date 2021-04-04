// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.DependencyInjection.Extensions
{
    public static partial class IScopedServiceCollectionExtensions
    {
        internal readonly static LifetimeServiceCollectionExtension<object, ScopedServiceDescriptor, IScopedServiceCollection> Extension =
            new LifetimeServiceCollectionExtension<object, ScopedServiceDescriptor, IScopedServiceCollection>(DescriptorActivator.Scoped);

        public static IScopedServiceCollection Add(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            Extension.Add(collection, descriptor);

        public static IScopedServiceCollection Add(this IScopedServiceCollection collection, IEnumerable<ScopedServiceDescriptor> descriptors) =>
            Extension.Add(collection, descriptors);

        public static IScopedServiceCollection RemoveAll(this IScopedServiceCollection collection, Type serviceType) =>
            Extension.RemoveAll(collection, serviceType);

        public static IScopedServiceCollection RemoveAll<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            Extension.RemoveAll<TService>(collection);

        public static IScopedServiceCollection Replace(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            Extension.Replace(collection, descriptor);

        public static bool Contains(this IScopedServiceCollection collection, Type serviceType) =>
            Extension.Contains(collection, serviceType);

        public static bool Contains<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            Extension.Contains<TService>(collection);

        public static void TryAdd(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            Extension.TryAdd(collection, descriptor);

        public static void TryAdd(this IScopedServiceCollection collection, IEnumerable<ScopedServiceDescriptor> descriptors) =>
            Extension.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this IScopedServiceCollection collection, ScopedServiceDescriptor descriptor) =>
            Extension.TryAddEnumerable(collection, descriptor);

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
            Extension.TryAddEnumerable(collection, descriptors);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service, Type implementationType) =>
            Extension.TryAddService(collection, service, implementationType);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service) =>
            Extension.TryAddService(collection, service);

        public static void TryAddScoped(this IScopedServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            Extension.TryAddService(collection, service, implementationFactory);

        public static void TryAddScoped<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            TryAddScoped(collection, typeof(TService));

        public static void TryAddScoped<TService, TImplementation>(this IScopedServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.TryAddService<TService, TImplementation>(collection);

        public static void TryAddScoped<TService>(this IScopedServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.TryAddService(collection, implementationFactory);

        /* BEGIN HELPERS */

        public static IScopedServiceCollection AddScoped<TService>(this IScopedServiceCollection collection)
            where TService : class =>
            Extension.AddService<TService>(collection);

        public static IScopedServiceCollection AddScoped(this IScopedServiceCollection collection, Type serviceType, Type implementationType) =>
            Extension.AddService(collection, serviceType, implementationType);

        public static IScopedServiceCollection AddScoped(this IScopedServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            Extension.AddService(collection, serviceType, implementationFactory);

        public static IScopedServiceCollection AddScoped<TService, TImplementation>(this IScopedServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection);

        public static IScopedServiceCollection AddScoped(this IScopedServiceCollection collection, Type serviceType) =>
            Extension.AddService(collection, serviceType);

        public static IScopedServiceCollection AddScoped<TService>(this IScopedServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.AddService(collection, implementationFactory);

        public static IScopedServiceCollection AddScoped<TService, TImplementation>(this IScopedServiceCollection collection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection, implementationFactory);

        /* END HELPERS */

        public static IServiceCollectionAdapter<IScopedServiceCollection> CreateServiceCollectionAdapter(IScopedServiceCollection collection) =>
            Extension.CreateServiceCollectionAdapter(collection);
    }
}
