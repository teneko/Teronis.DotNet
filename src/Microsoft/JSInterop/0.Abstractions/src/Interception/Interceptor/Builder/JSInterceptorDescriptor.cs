// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public class JSInterceptorDescriptor : ScopedServiceDescriptor
    {
        public readonly static DescriptorActivatorBase<JSInterceptorDescriptor> Activator = new JSInterceptorDescriptorActivator();

        public JSInterceptorDescriptor(Type serviceType)
            : base(serviceType) { }

        public JSInterceptorDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public JSInterceptorDescriptor(Type serviceType, Func<IServiceProvider, object> factory)
            : base(serviceType, factory) { }

        protected internal JSInterceptorDescriptor(ServiceDescriptor descriptor) 
            : base(descriptor) { }

        private class JSInterceptorDescriptorActivator : DescriptorActivatorBase<JSInterceptorDescriptor> {
            protected override JSInterceptorDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new JSInterceptorDescriptor(serviceDescriptor);

            protected override JSInterceptorDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new JSInterceptorDescriptor(serviceType, implementationType);

            protected override JSInterceptorDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                throw new NotSupportedException("The interceptor must be able to be created on the fly.");

            protected override JSInterceptorDescriptor CreateDescriptor(Type serviceType, Func<IServiceProvider, object> implementationFactory) =>
                new JSInterceptorDescriptor(serviceType, implementationFactory);
        }
    }
}
