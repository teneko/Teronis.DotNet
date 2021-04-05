// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Microsoft.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptors.Builder
{
    public static class JSInterceptorBuilderExtensions
    {
        /// <summary>
        /// Builds the interceptor.
        /// </summary>
        /// <param name="interceptorBuilder"></param>
        /// <param name="serviceProvider">
        /// If null the interceptor descriptors without implementation are
        /// skipped.
        /// </param>
        /// <returns></returns>
        internal static JSIterativeInterceptor Build(
            this JSInterceptorServiceCollection interceptorBuilder,
            IServiceProvider serviceProvider)
        {
            var interceptors = new List<IJSInterceptor>();

            foreach (var interceptorDescriptor in interceptorBuilder) {
                var interceptor = (IJSInterceptor)LifetimeServiceActivator.GetInstanceOrCreateInstance(serviceProvider, interceptorDescriptor);
                interceptors.Add(interceptor);
            }

            return new JSIterativeInterceptor(interceptors);
        }
    }
}
