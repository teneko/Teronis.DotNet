// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSObjectInterceptorWalkerBuilder
    {
        IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors { get; }

        IJSObjectInterceptorWalkerBuilder InsertInterceptor(int index, Type interceptorType);
        IJSObjectInterceptorWalkerBuilder InsertInterceptor(int index, IJSObjectInterceptor interceptor);

        IJSObjectInterceptorWalkerBuilder AddInterceptor(Type interceptorType);
        IJSObjectInterceptorWalkerBuilder AddInterceptor(IJSObjectInterceptor interceptor);

        IJSObjectInterceptorWalkerBuilder RemoveInterceptor(Type interceptorType);
        IJSObjectInterceptorWalkerBuilder RemoveInterceptor(IJSObjectInterceptor interceptor);
    }
}
