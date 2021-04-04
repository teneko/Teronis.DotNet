// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    internal sealed class ScopedServiceDescriptorActivator : DescriptorActivatorBase<ScopedServiceDescriptor>
    {
        internal protected override ScopedServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
            new ScopedServiceDescriptor(serviceDescriptor);

        internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
            new ScopedServiceDescriptor(serviceType, implementationType);

        internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
            throw new NotSupportedException("Cannot describe scoped service by instance");

        internal protected override ScopedServiceDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            new ScopedServiceDescriptor(serviceType, implementationFactory);
    }
}
