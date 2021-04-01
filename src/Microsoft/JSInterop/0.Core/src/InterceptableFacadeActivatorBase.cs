// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public abstract class InterceptableFacadeActivatorBase
    {
        private readonly BuildInterceptorDelegate? buildInterceptor;

        public InterceptableFacadeActivatorBase(ILateInterceptorBuilder? interceptorBuilder) =>
            buildInterceptor = interceptorBuilder is null ? null : (BuildInterceptorDelegate?)interceptorBuilder.BuildInterceptor;

        public IJSInterceptor BuildInterceptor(Action<IJSInterceptorBuilder>? configureBuilder) =>
            buildInterceptor
                ?.Invoke(configureBuilder: configureBuilder)
                ?? JSInterceptor.Default;
    }
}
