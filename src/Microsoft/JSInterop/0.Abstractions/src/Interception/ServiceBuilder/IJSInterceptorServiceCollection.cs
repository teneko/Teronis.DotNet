// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public interface IJSInterceptorServiceCollection : ILifetimeServiceCollection<JSInterceptorServiceDescriptor>
    {
        IJSInterceptorServiceCollection UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorServiceDescriptor, IJSInterceptorServiceCollection>> callback);
    }
}
