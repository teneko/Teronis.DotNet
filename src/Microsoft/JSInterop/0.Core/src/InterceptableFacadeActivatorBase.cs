// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Interception.Interceptor;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;

namespace Teronis.Microsoft.JSInterop
{
    public abstract class InterceptableFacadeActivatorBase
    {
        private readonly BuildInterceptorDelegate? buildInterceptor;

        public InterceptableFacadeActivatorBase(IJSInterceptorBuilder? interceptorBuilder) =>
            buildInterceptor = interceptorBuilder is null ? null : (BuildInterceptorDelegate?)interceptorBuilder.BuildInterceptor;

        public IJSInterceptor BuildInterceptor() =>
            buildInterceptor
                ?.Invoke()
                ?? JSInterceptor.Default;
    }
}
