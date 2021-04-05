// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder;
using Teronis.Microsoft.JSInterop.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class DynamicIInterceptorBuilderExtensions
    {
        /// <summary>
        /// Adds the default dynamic-only interceptors.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection AddDefaultDynamicInterceptors(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => {
                e.AddScoped<JSDynamicProxyActivatingInterceptor>();
            });

            return interceptorBuilder;
        }
    }
}
