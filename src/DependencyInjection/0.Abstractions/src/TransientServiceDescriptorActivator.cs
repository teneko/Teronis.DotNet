// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    internal sealed class TransientServiceDescriptorActivator : DescriptorActivatorBase<TransientServiceDescriptor>
    {
        internal protected override TransientServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
            new TransientServiceDescriptor(serviceDescriptor);

        internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
            new TransientServiceDescriptor(serviceType, implementationType);

        internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
            throw new NotSupportedException("Cannot describe transient service by instance");

        internal protected override TransientServiceDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
            new TransientServiceDescriptor(serviceType, implementationFactory);
    }
}
