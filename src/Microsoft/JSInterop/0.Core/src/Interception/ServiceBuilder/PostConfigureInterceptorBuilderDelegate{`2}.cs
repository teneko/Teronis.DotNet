// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public delegate void PostConfigureInterceptorBuilderDelegate<TDerivedInterceptorBuilderOptions, TDerivedValueAssignerOptions>(
        TDerivedInterceptorBuilderOptions interceptorBuilderOptions,
        TDerivedValueAssignerOptions propertyAssignerOptions)
        where TDerivedInterceptorBuilderOptions : JSInterceptorBuilderOptions<TDerivedInterceptorBuilderOptions>
        where TDerivedValueAssignerOptions : ValueAssignerOptions<TDerivedValueAssignerOptions>;
}
