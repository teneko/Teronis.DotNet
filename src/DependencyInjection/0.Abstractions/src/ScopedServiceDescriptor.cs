// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.DependencyInjection
{
    public class ScopedServiceDescriptor : NonSingletonServiceDescriptor
    {
        internal ScopedServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        public ScopedServiceDescriptor(Type singletonType)
            : base(singletonType) { }

        public ScopedServiceDescriptor(Type singletonType, Type implementationType)
            : base(singletonType, implementationType) { }

        public ScopedServiceDescriptor(Type singletonType, Func<IServiceProvider, object> factory)
            : base(singletonType, factory) { }
    }
}
