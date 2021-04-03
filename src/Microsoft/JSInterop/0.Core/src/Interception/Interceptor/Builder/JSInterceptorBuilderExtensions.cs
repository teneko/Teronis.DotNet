// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
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
        /// <param name="serviceProviderByRegistrationPhase">Provides service provider for each registration phase.</param>
        /// <returns></returns>
        internal static JSIterativeInterceptor Build(
            this JSInterceptorBuilder interceptorBuilder,
            IServiceProvider? serviceProvider,
            IReadOnlyDictionary<InterceptorDescriptorRegistrationPhase, IServiceProvider?>? serviceProviderByRegistrationPhase = null)
        {
            var interceptors = new List<IJSInterceptor>();

            foreach (var interceptorDescriptor in interceptorBuilder.InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                    continue;
                }

                var currentServiceProvider = serviceProviderByRegistrationPhase?.GetValueOrDefault(interceptorDescriptor.RegistrationPhase) ?? serviceProvider;

                if (currentServiceProvider is null) {
                    continue;
                }

                var interceptor = (IJSInterceptor)ActivatorUtilities.CreateInstance(currentServiceProvider, interceptorDescriptor.ImplementationType);
                interceptors.Add(interceptor);
            }

            return new JSIterativeInterceptor(interceptors);
        }
    }
}
