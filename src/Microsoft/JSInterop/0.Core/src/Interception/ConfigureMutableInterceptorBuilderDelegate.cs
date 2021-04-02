// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;
using Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public delegate void ConfigureMutableInterceptorBuilderDelegate<TDerivedInterceptorBuilderOptions, TDerivedValueAssignerOptions>(
        TDerivedInterceptorBuilderOptions interceptorBuilderOptions,
        TDerivedValueAssignerOptions propertyAssignerOptions)
        where TDerivedInterceptorBuilderOptions : JSInterceptorBuilderOptions<TDerivedInterceptorBuilderOptions>
        where TDerivedValueAssignerOptions : ValueAssignerOptions<TDerivedValueAssignerOptions>;
}
