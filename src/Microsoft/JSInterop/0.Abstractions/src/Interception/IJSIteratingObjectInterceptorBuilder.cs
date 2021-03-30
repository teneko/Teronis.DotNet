// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSIteratingObjectInterceptorBuilder
    {
        IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors { get; }

        IJSIteratingObjectInterceptorBuilder InsertInterceptor(int index, Type interceptorType);
        IJSIteratingObjectInterceptorBuilder InsertInterceptor(int index, IJSObjectInterceptor interceptor);

        IJSIteratingObjectInterceptorBuilder AddInterceptor(Type interceptorType);
        IJSIteratingObjectInterceptorBuilder AddInterceptor(IJSObjectInterceptor interceptor);

        IJSIteratingObjectInterceptorBuilder RemoveInterceptor(Type interceptorType);
        IJSIteratingObjectInterceptorBuilder RemoveInterceptor(IJSObjectInterceptor interceptor);
    }
}
