// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public class TransientServiceDescriptor : NonSingletonServiceDescriptor
    {
        internal TransientServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        public TransientServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public TransientServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public TransientServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory)
            : base(serviceType, factory) { }
    }
}
