// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class NonSingletonServiceDescriptor<TProvider> : LifetimeServiceDescriptor<TProvider>
        where TProvider : class, IServiceProvider
    {
        internal protected NonSingletonServiceDescriptor(ServiceDescriptor descriptor)
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

        public NonSingletonServiceDescriptor(Type singletonType, ImplementationFactoryDelegate<TProvider, object> factory)
            : base(singletonType, factory) { }
    }
}
