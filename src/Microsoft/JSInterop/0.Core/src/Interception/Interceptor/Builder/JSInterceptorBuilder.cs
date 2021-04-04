﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    internal sealed class JSInterceptorBuilder : LifetimeServiceCollection<JSInterceptorDescriptor>, IJSInterceptorServiceCollection
    {
        public override ServiceLifetime Lifetime =>
            ServiceLifetime.Scoped;

        protected override List<JSInterceptorDescriptor> Descriptors { get; }

        public JSInterceptorBuilder(IEnumerable<JSInterceptorDescriptor>? descriptors)
        {
            if (descriptors is null) {
                Descriptors = new List<JSInterceptorDescriptor>();
            } else {
                Descriptors = new List<JSInterceptorDescriptor>(descriptors);
            }
        }

        public JSInterceptorBuilder()
            : this(descriptors: null) { }

        public JSInterceptorBuilder UseExtension(Action<ScopedServiceCollectionInstanceExtension<IJSInterceptor, JSInterceptorDescriptor, JSInterceptorBuilder>> callback)
        {
            callback?.Invoke(new ScopedServiceCollectionInstanceExtension<IJSInterceptor, JSInterceptorDescriptor, JSInterceptorBuilder>(this, JSInterceptorDescriptor.Activator));
            return this;
        }

        IJSInterceptorServiceCollection IJSInterceptorServiceCollection.UseExtension(Action<ScopedServiceCollectionInstanceExtension<IJSInterceptor, JSInterceptorDescriptor, IJSInterceptorServiceCollection>> callback)
        {
            callback?.Invoke(new ScopedServiceCollectionInstanceExtension<IJSInterceptor, JSInterceptorDescriptor, IJSInterceptorServiceCollection>(this, JSInterceptorDescriptor.Activator));
            return this;
        }
    }
}
