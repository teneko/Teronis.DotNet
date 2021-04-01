// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSInterceptorBuilder
    {
        IReadOnlyList<JSInterceptorDescriptor> InterceptorDescriptors { get; }

        IJSInterceptorBuilder Insert(int index, Type interceptorType);
        IJSInterceptorBuilder Insert(int index, IJSInterceptor interceptor);

        IJSInterceptorBuilder Add(Type interceptorType);
        IJSInterceptorBuilder Add(IJSInterceptor interceptor);

        IJSInterceptorBuilder Remove(Type interceptorType);
        IJSInterceptorBuilder Remove(IJSInterceptor interceptor);
        IJSInterceptorBuilder RemoveAt(int index);
    }
}
