// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder
{
    public interface IJSInterceptorServiceCollection : ILifetimeServiceCollection<JSInterceptorDescriptor>
    {
        IJSInterceptorServiceCollection UseExtension(Action<ScopedServicesExtension<IJSInterceptor, IServiceProvider, JSInterceptorDescriptor, IJSInterceptorServiceCollection>> callback);
    }
}
