// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public static class InterceptorBuilderExtensions
    {
        /// <summary>
        /// Adds the default non-dynamic interceptors.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection AddDefaultNonDynamicInterceptors(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => e.AddScoped<JSLocalObjectActivatingInterceptor>());
            return interceptorBuilder;
        }

        /// <summary>
        /// Adds <see cref="JSIterativeValueAssignerInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection AddIterativeValueAssignerInterceptor(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => e.AddScoped<JSIterativeValueAssignerInterceptor>());
            return interceptorBuilder;
        }

        /// <summary>
        /// Removes first occurrence of <see cref="JSIterativeValueAssignerInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection RemoveIterativeValueAssignerInterceptor(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => e.RemoveAll<JSIterativeValueAssignerInterceptor>());
            return interceptorBuilder;
        }
    }
}
