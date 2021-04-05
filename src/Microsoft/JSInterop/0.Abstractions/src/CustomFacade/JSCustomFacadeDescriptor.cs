// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeServiceDescriptor : ScopedServiceDescriptor<IJSCustomFacadeFactoryServiceProvider>
    {
        public readonly static DescriptorActivatorBase<IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor> Activator = new JSCustomFacadeDescriptorActivator();

        public JSCustomFacadeServiceDescriptor(Type serviceType)
            : base(serviceType) { }

        public JSCustomFacadeServiceDescriptor(Type serviceType, Type implementationType)
            : base(serviceType, implementationType) { }

        public JSCustomFacadeServiceDescriptor(Type serviceType, ImplementationFactoryDelegate<IJSCustomFacadeFactoryServiceProvider, object> factory)
            : base(serviceType, factory) { }

        protected internal JSCustomFacadeServiceDescriptor(ServiceDescriptor descriptor)
            : base(descriptor) { }

        private class JSCustomFacadeDescriptorActivator : DescriptorActivatorBase<IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor>
        {
            protected override JSCustomFacadeServiceDescriptor CreateDescriptor(ServiceDescriptor serviceDescriptor) =>
                new JSCustomFacadeServiceDescriptor(serviceDescriptor);

            protected override JSCustomFacadeServiceDescriptor CreateDescriptor(Type serviceType, Type implementationType) =>
                new JSCustomFacadeServiceDescriptor(serviceType, implementationType);

            protected override JSCustomFacadeServiceDescriptor CreateDescriptor(Type serviceType, object implementationInstance) =>
                throw new NotSupportedException("The custom facade must be able to be created on the fly.");

            protected override JSCustomFacadeServiceDescriptor CreateDescriptor(Type serviceType, ImplementationFactoryDelegate<IJSCustomFacadeFactoryServiceProvider, object> implementationFactory) =>
                new JSCustomFacadeServiceDescriptor(serviceType, implementationFactory);
        }
    }
}
