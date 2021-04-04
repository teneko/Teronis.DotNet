// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.DependencyInjection
{
    public static class DescriptorActivator
    {
        public readonly static DescriptorActivatorBase<ScopedServiceDescriptor> Scoped = new ScopedServiceDescriptorActivator();
        public readonly static DescriptorActivatorBase<SingletonServiceDescriptor> Singleton = new SingletonServiceDescriptorActivator();
        public readonly static DescriptorActivatorBase<TransientServiceDescriptor> Transient = new TransientServiceDescriptorActivator();
    }
}
