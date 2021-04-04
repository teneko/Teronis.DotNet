// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.DependencyInjection.Extensions
{
    public static partial class ITransientServiceCollectionExtensions
    {
        internal readonly static LifetimeServiceCollectionExtension<object, TransientServiceDescriptor, ITransientServiceCollection> Extension =
            new LifetimeServiceCollectionExtension<object, TransientServiceDescriptor, ITransientServiceCollection>(DescriptorActivator.Transient);

        public static ITransientServiceCollection Add(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            Extension.Add(collection, descriptor);

        public static ITransientServiceCollection Add(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            Extension.Add(collection, descriptors);

        public static ITransientServiceCollection RemoveAll(this ITransientServiceCollection collection, Type serviceType) =>
            Extension.RemoveAll(collection, serviceType);

        public static ITransientServiceCollection RemoveAll<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            Extension.RemoveAll<TService>(collection);

        public static ITransientServiceCollection Replace(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            Extension.Replace(collection, descriptor);

        public static bool Contains(this ITransientServiceCollection collection, Type serviceType) =>
            Extension.Contains(collection, serviceType);

        public static bool Contains<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            Extension.Contains<TService>(collection);

        public static void TryAdd(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
            Extension.TryAdd(collection, descriptor);

        public static void TryAdd(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            Extension.TryAdd(collection, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <paramref name="collection."/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public static void TryAddEnumerable(this ITransientServiceCollection collection, TransientServiceDescriptor descriptor) =>
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
        public static void TryAddEnumerable(this ITransientServiceCollection collection, IEnumerable<TransientServiceDescriptor> descriptors) =>
            Extension.TryAddEnumerable(collection, descriptors);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service, Type implementationType) =>
            Extension.TryAddService(collection, service, implementationType);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service) =>
            Extension.TryAddService(collection, service);

        public static void TryAddTransient(this ITransientServiceCollection collection, Type service, Func<IServiceProvider, object> implementationFactory) =>
            Extension.TryAddService(collection, service, implementationFactory);

        public static void TryAddTransient<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            TryAddTransient(collection, typeof(TService));

        public static void TryAddTransient<TService, TImplementation>(this ITransientServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.TryAddService<TService, TImplementation>(collection);

        public static void TryAddTransient<TService>(this ITransientServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.TryAddService(collection, implementationFactory);

        /* BEGIN HELPERS */

        public static ITransientServiceCollection AddTransient<TService>(this ITransientServiceCollection collection)
            where TService : class =>
            Extension.AddService<TService>(collection);

        public static ITransientServiceCollection AddTransient(this ITransientServiceCollection collection, Type serviceType, Type implementationType) =>
            Extension.AddService(collection, serviceType, implementationType);

        public static ITransientServiceCollection AddTransient(this ITransientServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            Extension.AddService(collection, serviceType, implementationFactory);

        public static ITransientServiceCollection AddTransient<TService, TImplementation>(this ITransientServiceCollection collection)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection);

        public static ITransientServiceCollection AddTransient(this ITransientServiceCollection collection, Type serviceType) =>
            Extension.AddService(collection, serviceType);

        public static ITransientServiceCollection AddTransient<TService>(this ITransientServiceCollection collection, Func<IServiceProvider, TService> implementationFactory)
            where TService : class =>
            Extension.AddService(collection, implementationFactory);

        public static ITransientServiceCollection AddTransient<TService, TImplementation>(this ITransientServiceCollection collection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(collection, implementationFactory);

        /* END HELPERS */

        public static IServiceCollectionAdapter<ITransientServiceCollection> CreateServiceCollectionAdapter(ITransientServiceCollection collection) =>
            Extension.CreateServiceCollectionAdapter(collection);
    }
}
