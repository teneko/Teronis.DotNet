// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static partial class IScopedServiceCollectionExtensions
    {
        public static ISingletonServiceCollection UseExtension(this ISingletonServiceCollection collection, Action<ScopedServicesExtension<object, IServiceProvider, SingletonServiceDescriptor, ISingletonServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<object, IServiceProvider, SingletonServiceDescriptor, ISingletonServiceCollection>(collection, DescriptorActivator.Singleton));
            return collection;
        }
    }
}
