// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder
{
    internal sealed class JSInterceptorServiceCollection : LifetimeServiceCollection<IServiceProvider, JSInterceptorDescriptor>, IJSInterceptorServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        protected override List<JSInterceptorDescriptor> Descriptors { get; }

        public JSInterceptorServiceCollection(IEnumerable<JSInterceptorDescriptor>? descriptors)
            : base(typeof(IJSInterceptor))
        {
            if (descriptors is null) {
                Descriptors = new List<JSInterceptorDescriptor>();
            } else {
                Descriptors = new List<JSInterceptorDescriptor>(descriptors);
            }
        }

        public JSInterceptorServiceCollection()
            : this(descriptors: null) { }

        public JSInterceptorServiceCollection UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorDescriptor, JSInterceptorServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorDescriptor, JSInterceptorServiceCollection>(this, JSInterceptorDescriptor.Activator));
            return this;
        }

        IJSInterceptorServiceCollection IJSInterceptorServiceCollection.UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorDescriptor, IJSInterceptorServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorDescriptor, IJSInterceptorServiceCollection>(this, JSInterceptorDescriptor.Activator));
            return this;
        }
    }
}
