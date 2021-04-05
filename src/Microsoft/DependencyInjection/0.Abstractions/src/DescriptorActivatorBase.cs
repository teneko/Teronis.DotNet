// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public abstract class DescriptorActivatorBase<TProvider, TDescriptor>
        where TProvider : class, IServiceProvider
    {
        internal protected abstract TDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, Type implementationType);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, object implementationInstance);
        internal protected abstract TDescriptor CreateDescriptor(Type serviceType, ImplementationFactoryDelegate<TProvider, object> implementationFactory);
    }
}
