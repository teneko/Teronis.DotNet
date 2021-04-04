// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder
{
    public class ValueAssignerServiceCollection : LifetimeServiceCollection<ScopedServiceDescriptor>
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        public ValueAssignerServiceCollection()
            : base(typeof(IValueAssigner)) { }

        public void UseExtension(Action<ScopedServiceCollectionInstanceExtension<IValueAssigner, ScopedServiceDescriptor, ValueAssignerServiceCollection>> callback) =>
            callback?.Invoke(new ScopedServiceCollectionInstanceExtension<IValueAssigner, ScopedServiceDescriptor, ValueAssignerServiceCollection>(this, DescriptorActivator.Scoped));
    }
}
