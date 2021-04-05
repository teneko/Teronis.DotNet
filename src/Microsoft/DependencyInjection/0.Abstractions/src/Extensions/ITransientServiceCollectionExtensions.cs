﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection.Extensions
{
    public static partial class ITransientServiceCollectionExtensions
    {
        public static ISingletonServiceCollection UseExtension(this ISingletonServiceCollection collection, Action<TransientServicesExtension<object, IServiceProvider, SingletonServiceDescriptor, ISingletonServiceCollection>> callback)
        {
            callback?.Invoke(new TransientServicesExtension<object, IServiceProvider, SingletonServiceDescriptor, ISingletonServiceCollection>(collection, DescriptorActivator.Singleton));
            return collection;
        }
    }
}
