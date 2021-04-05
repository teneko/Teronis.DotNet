// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Component.Assigners;

namespace Teronis.Microsoft.JSInterop.Component.ServiceBuilder
{
    public class ValueAssignerServiceCollection : LifetimeServiceCollection<IServiceProvider, ScopedServiceDescriptor>
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        public ValueAssignerServiceCollection()
            : base(typeof(IValueAssigner)) { }

        public void UseExtension(Action<ScopedServicesExtension<IValueAssigner, IServiceProvider, ScopedServiceDescriptor, ValueAssignerServiceCollection>> callback) =>
            callback?.Invoke(new ScopedServicesExtension<IValueAssigner, IServiceProvider, ScopedServiceDescriptor, ValueAssignerServiceCollection>(this, DescriptorActivator.Scoped));
    }
}
