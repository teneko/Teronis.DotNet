// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static class TransientServicesExtensionExtensions
    {
        internal static IServiceCollectionAdapter<TCollection> CreateServiceCollectionAdapter<TServiceBase, TDescriptor, TCollection>(
            this TransientServicesExtension<TServiceBase, IServiceProvider, TDescriptor, TCollection> extension)
            where TServiceBase : class
            where TDescriptor : LifetimeServiceDescriptor<IServiceProvider>
            where TCollection : ILifetimeServiceCollection<TDescriptor> =>
            new ServiceCollectionAdapter<TDescriptor, TCollection>(extension.Services, extension.Extension.DescriptorActivator);
    }
}
