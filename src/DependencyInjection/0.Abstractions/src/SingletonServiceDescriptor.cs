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

        public SingletonServiceDescriptor(Type singletonType)
            : base(singletonType) { }

        public SingletonServiceDescriptor(Type singletonType, Type implementationType)
            : base(singletonType, implementationType) { }

        public SingletonServiceDescriptor(Type singletonType, object instance)
            : base(singletonType, instance) { }

        public SingletonServiceDescriptor(Type singletonType, Func<IServiceProvider, object> factory)
            : base(singletonType, factory) { }
    }
}
