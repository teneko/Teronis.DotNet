// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    internal sealed class JSInterceptorServiceCollection : LifetimeServiceCollection<IServiceProvider, JSInterceptorServiceDescriptor>, IJSInterceptorServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        protected override List<JSInterceptorServiceDescriptor> Descriptors { get; }

        public JSInterceptorServiceCollection(IEnumerable<JSInterceptorServiceDescriptor>? descriptors)
            : base(typeof(IJSInterceptor))
        {
            if (descriptors is null) {
                Descriptors = new List<JSInterceptorServiceDescriptor>();
            } else {
                Descriptors = new List<JSInterceptorServiceDescriptor>(descriptors);
            }
        }

        public JSInterceptorServiceCollection()
            : this(descriptors: null) { }

        public JSInterceptorServiceCollection UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorServiceDescriptor, JSInterceptorServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorServiceDescriptor, JSInterceptorServiceCollection>(this, JSInterceptorServiceDescriptor.Activator));
            return this;
        }

        IJSInterceptorServiceCollection IJSInterceptorServiceCollection.UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorServiceDescriptor, IJSInterceptorServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorServiceDescriptor, IJSInterceptorServiceCollection>(this, JSInterceptorServiceDescriptor.Activator));
            return this;
        }
    }
}
