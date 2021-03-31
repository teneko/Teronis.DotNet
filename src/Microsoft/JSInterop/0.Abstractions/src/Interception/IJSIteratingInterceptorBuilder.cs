// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSIteratingInterceptorBuilder
    {
        IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors { get; }

        IJSIteratingInterceptorBuilder Insert(int index, Type interceptorType);
        IJSIteratingInterceptorBuilder Insert(int index, IJSObjectInterceptor interceptor);

        IJSIteratingInterceptorBuilder Add(Type interceptorType);
        IJSIteratingInterceptorBuilder Add(IJSObjectInterceptor interceptor);

        IJSIteratingInterceptorBuilder Remove(Type interceptorType);
        IJSIteratingInterceptorBuilder Remove(IJSObjectInterceptor interceptor);
        IJSIteratingInterceptorBuilder RemoveAt(int index);
    }
}
