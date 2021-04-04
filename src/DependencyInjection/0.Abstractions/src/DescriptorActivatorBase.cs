// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public abstract class DescriptorActivatorBase<TDescriptor>
    {
        internal protected abstract TDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, Type implementationType);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, object implementationInstance);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory);
    }
}
