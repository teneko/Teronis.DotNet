// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
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
            interceptorBuilder.UseExtension(e => e.AddScoped<JSLocalObjectInterceptor>());
            return interceptorBuilder;
        }

        /// <summary>
        /// Adds <see cref="JSIterativeValueAssigningInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection AddIterativeValueAssignerInterceptor(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => e.AddScoped<JSIterativeValueAssigningInterceptor>());
            return interceptorBuilder;
        }

        /// <summary>
        /// Removes first occurrence of <see cref="JSIterativeValueAssigningInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorServiceCollection RemoveIterativeValueAssignerInterceptor(this IJSInterceptorServiceCollection interceptorBuilder)
        {
            interceptorBuilder.UseExtension(e => e.RemoveAll<JSIterativeValueAssigningInterceptor>());
            return interceptorBuilder;
        }
    }
}
