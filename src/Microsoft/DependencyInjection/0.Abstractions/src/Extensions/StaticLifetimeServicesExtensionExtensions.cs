// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static class StaticLifetimeServicesExtensionExtensions
    {
        internal static IServiceCollectionAdapter<TCollection> CreateServiceCollectionAdapter<TServiceBase, TDescriptor, TCollection>(
            this StaticLifetimeServicesExtension<TServiceBase, IServiceProvider, TDescriptor, TCollection> extension,
            TCollection collection)
            where TServiceBase : class
            where TDescriptor : LifetimeServiceDescriptor<IServiceProvider>
            where TCollection : ILifetimeServiceCollection<TDescriptor> =>
            new ServiceCollectionAdapter<TDescriptor, TCollection>(collection, extension.DescriptorActivator);
    }
}
