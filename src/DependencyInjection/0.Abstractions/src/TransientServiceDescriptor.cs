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

        public TransientServiceDescriptor(Type singletonType)
            : base(singletonType) { }

        public TransientServiceDescriptor(Type singletonType, Type implementationType)
            : base(singletonType, implementationType) { }

        public TransientServiceDescriptor(Type singletonType, Func<IServiceProvider, object> factory)
            : base(singletonType, factory) { }
    }
}
