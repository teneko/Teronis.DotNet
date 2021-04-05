// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public readonly struct ScopedServicesExtension<TServiceBase, TProvider, TDescriptor, TCollection>
        where TServiceBase : class
        where TProvider : class, IServiceProvider
        where TDescriptor : LifetimeServiceDescriptor<TProvider>
        where TCollection : ILifetimeServiceCollection<TDescriptor>
    {
        public readonly TCollection Services;

        internal readonly StaticLifetimeServicesExtension<TServiceBase, TProvider, TDescriptor, TCollection> Extension;

        public ScopedServicesExtension(
            TCollection services,
            DescriptorActivatorBase<TProvider, TDescriptor> descriptorActivator)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Extension = new StaticLifetimeServicesExtension<TServiceBase, TProvider, TDescriptor, TCollection>(descriptorActivator);
        }

        public void Add(TDescriptor descriptor) =>
            Extension.Add(Services, descriptor);

        public void Add(IEnumerable<TDescriptor> descriptors) =>
            Extension.Add(Services, descriptors);

        public void RemoveAll(Type serviceType) =>
            Extension.RemoveAll(Services, serviceType);

        public void RemoveAll<TService>()
            where TService : class, TServiceBase =>
            Extension.RemoveAll<TService>(Services);

        public void Replace(TDescriptor descriptor) =>
            Extension.Replace(Services, descriptor);

        public bool Contains(Type serviceType) =>
            Extension.Contains(Services, serviceType);

        public bool Contains<TService>()
            where TService : class, TServiceBase =>
            Extension.Contains<TService>(Services);

        public void TryAdd(TDescriptor descriptor) =>
            Extension.TryAdd(Services, descriptor);

        public void TryAdd(IEnumerable<TDescriptor> descriptors) =>
            Extension.TryAdd(Services, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <see cref="Services"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public void TryAddEnumerable(TDescriptor descriptor) =>
            Extension.TryAddEnumerable(Services, descriptor);

        /// <summary>
        /// Adds <paramref name="descriptors"/> if not a single descriptor with same
        /// service type and same implementation does exist in <see cref="Services"/>.
        /// </summary>
        /// <param name="descriptors"></param>
        /// <remarks>
        /// Prevents the registration of implementation type duplicates.
        /// </remarks>
        public void TryAddEnumerable(IEnumerable<TDescriptor> descriptors) =>
            Extension.TryAddEnumerable(Services, descriptors);

        public void TryAddScoped(Type service, Type implementationType) =>
            Extension.TryAddService(Services, service, implementationType);

        public void TryAddScoped(Type service) =>
            Extension.TryAddService(Services, service);

        public void TryAddScoped(Type service, ImplementationFactoryDelegate<IServiceProvider, object> implementationFactory) =>
            Extension.TryAddService(Services, service, implementationFactory);

        public void TryAddScoped<TService>()
            where TService : class, TServiceBase =>
            Extension.TryAddService<TService>(Services);

        public void TryAddScoped<TService, TImplementation>()
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            Extension.TryAddService<TService, TImplementation>(Services);

        public void TryAddScoped<TService>(ImplementationFactoryDelegate<IServiceProvider, TService> implementationFactory)
            where TService : class, TServiceBase =>
            Extension.TryAddService(Services, implementationFactory);

        /* BEGIN HELPERS */

        public void AddScoped<TService, TImplementation>(ImplementationFactoryDelegate<IServiceProvider, TImplementation> implementationFactory)
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(Services, implementationFactory);

        public void AddScoped<TService>(ImplementationFactoryDelegate<IServiceProvider, TService> implementationFactory)
            where TService : class, TServiceBase =>
            Extension.AddService(Services, implementationFactory);

        public void AddScoped<TService>()
            where TService : class, TServiceBase =>
            Extension.AddService<TService>(Services);

        public void AddScoped(Type serviceType) =>
            Extension.AddService(Services, serviceType);

        public void AddScoped<TService, TImplementation>()
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            Extension.AddService<TService, TImplementation>(Services);

        public void AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            Extension.AddService(Services, serviceType, implementationFactory);

        public void AddScoped(Type serviceType, Type implementationType) =>
            Extension.AddService(Services, serviceType, implementationType);

        /* END HELPERS */
    }
}
