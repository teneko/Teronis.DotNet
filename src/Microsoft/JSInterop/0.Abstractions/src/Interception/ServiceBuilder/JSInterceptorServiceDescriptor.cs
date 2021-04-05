// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public class JSInterceptorServiceDescriptor : ScopedServiceDescriptor
    {
        public readonly static DescriptorActivatorBase<IServiceProvider, JSInterceptorServiceDescriptor> Activator = new JSInterceptorDescriptorActivator();

        public JSInterceptorServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public JSInterceptorServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public JSInterceptorServiceDescriptor(Type serviceType, ImplementationFactoryDelegate<IServiceProvider, object> factory)
            : base(serviceType, factory) { }

        protected internal JSInterceptorServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        private class JSInterceptorDescriptorActivator : DescriptorActivatorBase<IServiceProvider, JSInterceptorServiceDescriptor>
        {
            protected override JSInterceptorServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new JSInterceptorServiceDescriptor(serviceDescriptor);

            protected override JSInterceptorServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new JSInterceptorServiceDescriptor(serviceType, implementationType);

            protected override JSInterceptorServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                throw new NotSupportedException("The interceptor must be able to be created on the fly.");

            protected override JSInterceptorServiceDescriptor CreateDescriptor(Type serviceType, ImplementationFactoryDelegate<IServiceProvider, object> implementationFactory) =>
                new JSInterceptorServiceDescriptor(serviceType, implementationFactory);
        }
    }
}
