// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static partial class ISingletonServiceCollectionExtensions
    {
        internal readonly static LifetimeServiceCollectionExtension<object, SingletonServiceDescriptor, ISingletonServiceCollection> Extension =
            new LifetimeServiceCollectionExtension<object, SingletonServiceDescriptor, ISingletonServiceCollection>(DescriptorActivator.Singleton);

        public static ISingletonServiceCollection Add(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            Extension.Add(collection, descriptor);

        public static ISingletonServiceCollection Add(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            Extension.Add(collection, descriptors);

        public static ISingletonServiceCollection RemoveAll(this ISingletonServiceCollection collection, Type serviceType) =>
            Extension.RemoveAll(collection, serviceType);

        public static ISingletonServiceCollection RemoveAll<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            Extension.RemoveAll<TService>(collection);

        public static ISingletonServiceCollection Replace(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            Extension.Replace(collection, descriptor);

        public static bool Contains(this ISingletonServiceCollection collection, Type serviceType) =>
            Extension.Contains(collection, serviceType);

        public static bool Contains<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            Extension.Contains<TService>(collection);

        public static void TryAdd(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
            Extension.TryAdd(collection, descriptor);

        public static void TryAdd(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            Extension.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this ISingletonServiceCollection collection, SingletonServiceDescriptor descriptor) =>
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
        public static void TryAddEnumerable(this ISingletonServiceCollection collection, IEnumerable<SingletonServiceDescriptor> descriptors) =>
            Extension.TryAddEnumerable(collection, descriptors);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service, Type implementationType) =>
            Extension.TryAddService(collection, service, implementationType);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service) =>
            Extension.TryAddService(collection, service);

        public static void TryAddSingleton(this ISingletonServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            Extension.TryAddService(collection, service, implementationFactory);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            TryAddSingleton(collection, typeof(TService));

        public static void TryAddSingleton<TService, TImplementation>(this ISingletonServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.TryAddService<TService, TImplementation>(collection);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection, TService instance)
            where TService : class =>
            Extension.TryAddService(collection, instance);

        public static void TryAddSingleton<TService>(this ISingletonServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.TryAddService(collection, implementationFactory);

        /* BEGIN HELPERS */

        public static ISingletonServiceCollection AddSingleton<TService>(this ISingletonServiceCollection collection)
            where TService : class =>
            Extension.AddService<TService>(collection);

        public static ISingletonServiceCollection AddSingleton(this ISingletonServiceCollection collection, Type serviceType, Type implementationType) =>
            Extension.AddService(collection, serviceType, implementationType);

        public static ISingletonServiceCollection AddSingleton(this ISingletonServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            Extension.AddService(collection, serviceType, implementationFactory);

        public static ISingletonServiceCollection AddSingleton<TService, TImplementation>(this ISingletonServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection);

        public static ISingletonServiceCollection AddSingleton(this ISingletonServiceCollection collection, Type serviceType) =>
            Extension.AddService(collection, serviceType);

        public static ISingletonServiceCollection AddSingleton<TService>(this ISingletonServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.AddService(collection, implementationFactory);

        public static ISingletonServiceCollection AddSingleton<TService, TImplementation>(this ISingletonServiceCollection collection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection, implementationFactory);

        public static ISingletonServiceCollection AddSingleton<TService>(this ISingletonServiceCollection collection, TService implementationInstance)
            where TService : class =>
            Extension.AddService(collection, implementationInstance);

        public static ISingletonServiceCollection AddSingleton(this ISingletonServiceCollection collection, Type serviceType, object implementationInstance) =>
            Extension.AddService(collection, serviceType, implementationInstance);

        /* END HELPERS */

        public static IServiceCollectionAdapter<ISingletonServiceCollection> CreateServiceCollectionAdapter(ISingletonServiceCollection collection) =>
            Extension.CreateServiceCollectionAdapter(collection);
    }
}
