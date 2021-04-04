﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.DependencyInjection;
using Teronis.Microsoft.DependencyInjection.Extensions;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public interface IJSInterceptorServiceCollection : ILifetimeServiceCollection<JSInterceptorDescriptor>
    {
        IJSInterceptorServiceCollection UseExtension(Action<ScopedServiceCollectionInstanceExtension<IJSInterceptor, JSInterceptorDescriptor, IJSInterceptorServiceCollection>> callback);
    }
}
