// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public class SingletonServiceDescriptor : LifetimeServiceDescriptor
    {
        internal SingletonServiceDescriptor(ServiceDescriptor serviceDescriptor)
            : base(serviceDescriptor) { }

        public SingletonServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public SingletonServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public SingletonServiceDescriptor(Type serviceType, object instance)
            : base(serviceType, instance) { }

        public SingletonServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory)
            : base(serviceType, factory) { }
    }
}
