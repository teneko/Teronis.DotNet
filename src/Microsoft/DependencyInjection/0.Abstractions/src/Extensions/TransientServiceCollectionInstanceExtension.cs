// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public readonly struct TransientServiceCollectionInstanceExtension<TServiceBase, TDescriptor, TCollection>
        where TServiceBase : class
        where TDescriptor : LifetimeServiceDescriptor
        where TCollection : ILifetimeServiceCollection<TDescriptor>
    {
        public readonly TCollection Services;
        private readonly LifetimeServiceCollectionExtension<TServiceBase, TDescriptor, TCollection> extensionsTemplate;

        internal TransientServiceCollectionInstanceExtension(
            TCollection services,
            DescriptorActivatorBase<TDescriptor> descriptorActivator)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            extensionsTemplate = new LifetimeServiceCollectionExtension<TServiceBase, TDescriptor, TCollection>(descriptorActivator);
        }

        public void Add(TDescriptor descriptor) =>
            extensionsTemplate.Add(Services, descriptor);

        public void Add(IEnumerable<TDescriptor> descriptors) =>
            extensionsTemplate.Add(Services, descriptors);

        public void RemoveAll(Type serviceType) =>
            extensionsTemplate.RemoveAll(Services, serviceType);

        public void RemoveAll<TService>()
            where TService : class, TServiceBase =>
            extensionsTemplate.RemoveAll<TService>(Services);

        public void Replace(TDescriptor descriptor) =>
            extensionsTemplate.Replace(Services, descriptor);

        public bool Contains(Type serviceType) =>
            extensionsTemplate.Contains(Services, serviceType);

        public bool Contains<TService>()
            where TService : class, TServiceBase =>
            extensionsTemplate.Contains<TService>(Services);

        public void TryAdd(TDescriptor descriptor) =>
            extensionsTemplate.TryAdd(Services, descriptor);

        public void TryAdd(IEnumerable<TDescriptor> descriptors) =>
            extensionsTemplate.TryAdd(Services, descriptors);

        /// <summary>
        /// Adds <paramref name="descriptor"/> if an existing descriptor with the same
        /// service type and an implementation that does not already exist in 
        /// <see cref="Services"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <remarks>Prevents the registration of implementation type duplicates.</remarks>
        public void TryAddEnumerable(TDescriptor descriptor) =>
            extensionsTemplate.TryAddEnumerable(Services, descriptor);

        /// <summary>
        /// Adds <paramref name="descriptors"/> if not a single descriptor with same
        /// service type and same implementation does exist in <see cref="Services"/>.
        /// </summary>
        /// <param name="descriptors"></param>
        /// <remarks>
        /// Prevents the registration of implementation type duplicates.
        /// </remarks>
        public void TryAddEnumerable(IEnumerable<TDescriptor> descriptors) =>
            extensionsTemplate.TryAddEnumerable(Services, descriptors);

        public void TryAddTransient(Type service, Type implementationType) =>
            extensionsTemplate.TryAddService(Services, service, implementationType);

        public void TryAddTransient(Type service) =>
            extensionsTemplate.TryAddService(Services, service);

        public void TryAddTransient(Type service, Func<IServiceProvider, object> implementationFactory) =>
            extensionsTemplate.TryAddService(Services, service, implementationFactory);

        public void TryAddTransient<TService>()
            where TService : class, TServiceBase =>
            extensionsTemplate.TryAddService<TService>(Services);

        public void TryAddTransient<TService, TImplementation>()
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            extensionsTemplate.TryAddService<TService, TImplementation>(Services);

        public void TryAddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class, TServiceBase =>
            extensionsTemplate.TryAddService(Services, implementationFactory);

        /* BEGIN HELPERS */

        public void AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            extensionsTemplate.AddService<TService, TImplementation>(Services, implementationFactory);

        public void AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class, TServiceBase =>
            extensionsTemplate.AddService(Services, implementationFactory);

        public void AddTransient<TService>()
            where TService : class, TServiceBase =>
            extensionsTemplate.AddService<TService>(Services);

        public void AddTransient(Type serviceType) =>
            extensionsTemplate.AddService(Services, serviceType);

        public void AddTransient<TService, TImplementation>()
            where TService : class, TServiceBase
            where TImplementation : class, TService =>
            extensionsTemplate.AddService<TService, TImplementation>(Services);

        public void AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            extensionsTemplate.AddService(Services, serviceType, implementationFactory);

        public void AddTransient(Type serviceType, Type implementationType) =>
            extensionsTemplate.AddService(Services, serviceType, implementationType);

        /* END HELPERS */

        public IServiceCollectionAdapter<TCollection> CreateServiceCollectionAdapter() =>
            extensionsTemplate.CreateServiceCollectionAdapter(Services);
    }
}
