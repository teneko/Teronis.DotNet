// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.Interception.Interceptor;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
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

        /// <summary>
        /// Adds <see cref="JSIterativeValueAssignerInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorBuilder AddIterativeValueAssignerInterceptor(this IJSInterceptorBuilder interceptorBuilder)
        {
            interceptorBuilder.Add(typeof(JSIterativeValueAssignerInterceptor));
            return interceptorBuilder;
        }

        /// <summary>
        /// Removes first occurrence of <see cref="JSIterativeValueAssignerInterceptor"/>.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <returns></returns>
        public static IJSInterceptorBuilder RemoveIterativeValueAssignerInterceptor(this IJSInterceptorBuilder interceptorBuilder)
        {
            interceptorBuilder.Remove(typeof(JSIterativeValueAssignerInterceptor));
            return interceptorBuilder;
        }
    }
}
