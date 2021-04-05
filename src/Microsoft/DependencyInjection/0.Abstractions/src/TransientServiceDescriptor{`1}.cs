// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.DependencyInjection
{
    public class TransientServiceDescriptor<TProvider> : NonSingletonServiceDescriptor<TProvider>
        where TProvider : class, IServiceProvider
    {
        internal TransientServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        public TransientServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public TransientServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public TransientServiceDescriptor(Type serviceType, ImplementationFactoryDelegate<TProvider, object> factory)
            : base(serviceType, factory) { }
    }
}
