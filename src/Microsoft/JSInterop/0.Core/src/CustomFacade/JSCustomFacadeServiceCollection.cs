// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeServiceCollection : LifetimeServiceCollection<IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor>, IJSCustomFacadeServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        public JSCustomFacadeServiceCollection()
            : base(typeof(IAsyncDisposable)) { }

        public IJSCustomFacadeServiceCollection UseExtension(Action<ScopedServicesExtension<IAsyncDisposable, IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor, IJSCustomFacadeServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<IAsyncDisposable, IJSCustomFacadeFactoryServiceProvider, JSCustomFacadeServiceDescriptor, IJSCustomFacadeServiceCollection>(this, JSCustomFacadeServiceDescriptor.Activator));
            return this;
        }
    }
}
