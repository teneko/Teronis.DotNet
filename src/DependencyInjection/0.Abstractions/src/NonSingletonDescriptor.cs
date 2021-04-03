// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public class NonSingletonServiceDescriptor : LifetimeServiceDescriptor
    {
        internal NonSingletonServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor)
        {
            if (!(descriptor.ImplementationInstance is null)) { 
                throw new ArgumentException("Cannot describe non-singleton service by instance.");
            }
        }

        public NonSingletonServiceDescriptor(Type singletonType) 
            : base(singletonType) { }

        public NonSingletonServiceDescriptor(Type singletonType, Type implementationType)
            : base(singletonType, implementationType) { }

        public NonSingletonServiceDescriptor(Type singletonType, Func<IServiceProvider, object> factory)
            : base(singletonType, factory) { }
    }
}
