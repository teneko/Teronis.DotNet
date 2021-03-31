// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public delegate void LateConfigureInterceptorBuilderDelegate<TDerivedInterceptorBuilderOptions, TDerivedPropertyAssignerOptions>(
        TDerivedInterceptorBuilderOptions interceptorBuilderOptions,
        TDerivedPropertyAssignerOptions propertyAssignerOptions)
        where TDerivedInterceptorBuilderOptions : JSInterceptorBuilderOptions<TDerivedInterceptorBuilderOptions>
        where TDerivedPropertyAssignerOptions : PropertyAssignerOptions<TDerivedPropertyAssignerOptions>;
}
