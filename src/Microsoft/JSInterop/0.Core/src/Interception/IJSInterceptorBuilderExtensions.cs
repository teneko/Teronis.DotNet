// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class InterceptorBuilderExtensions
    {
        /// <summary>
        /// Adds the default non-dynamic interceptors.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorBuilder AddDefaultNonDynamicInterceptors(this IJSInterceptorBuilder interceptorBuilder)
        {
            interceptorBuilder.Add(typeof(JSLocalObjectActivatingInterceptor));
            return interceptorBuilder;
        }
    }
}
