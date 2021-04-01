// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class DynamicIInterceptorBuilderExtensions
    {
        /// <summary>
        /// Adds the default dynamic-only interceptors.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorBuilder AddDefaultDynamicInterceptors(this IJSInterceptorBuilder interceptorBuilder)
        {
            interceptorBuilder.Add(typeof(JSDynamicProxyActivatingInterceptor));
            interceptorBuilder.Add(typeof(JSDynamicModuleActivatingInterceptor));
            return interceptorBuilder;
        }
    }
}
