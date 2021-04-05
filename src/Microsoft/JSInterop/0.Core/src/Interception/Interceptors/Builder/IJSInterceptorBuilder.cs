// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder
{
    public interface IJSInterceptorBuilder
    {
        IJSInterceptor BuildInterceptor();
    }
}
