// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.DependencyInjection
{
    public static class DescriptorActivator
    {
        public readonly static DescriptorActivatorBase<IServiceProvider, ScopedServiceDescriptor> Scoped = new ScopedServiceDescriptorActivator();
        public readonly static DescriptorActivatorBase<IServiceProvider, SingletonServiceDescriptor> Singleton = new SingletonServiceDescriptorActivator();
        public readonly static DescriptorActivatorBase<IServiceProvider, TransientServiceDescriptor> Transient = new TransientServiceDescriptorActivator();
    }
}
