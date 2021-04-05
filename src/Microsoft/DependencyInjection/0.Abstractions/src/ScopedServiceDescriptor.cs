// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class ScopedServiceDescriptor : ScopedServiceDescriptor<IServiceProvider>
    {
        internal protected ScopedServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        public ScopedServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public ScopedServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public ScopedServiceDescriptor(Type serviceType, ImplementationFactoryDelegate<IServiceProvider, object> factory)
            : base(serviceType, factory) { }
    }
}
