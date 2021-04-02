// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    internal sealed class JSInterceptorBuilder : IJSInterceptorBuilder
    {
        public IReadOnlyList<JSInterceptorDescriptor> InterceptorDescriptors =>
            interceptorDescriptors;

        private List<JSInterceptorDescriptor> interceptorDescriptors;
        private InterceptorDescriptorRegistrationPhase currentRegistrationPhase;

        public JSInterceptorBuilder() =>
            interceptorDescriptors = new List<JSInterceptorDescriptor>();

        public JSInterceptorBuilder(IEnumerable<JSInterceptorDescriptor> interceptorDescriptors) =>
            this.interceptorDescriptors = new List<JSInterceptorDescriptor>(interceptorDescriptors);

        /// <summary>
        /// Sets the current registration phase.
        /// </summary>
        /// <param name="registrationPhase"></param>
        public void SetRegistrationPhase(InterceptorDescriptorRegistrationPhase registrationPhase) =>
            currentRegistrationPhase = registrationPhase;

        private Type GetInterceptorType(IJSInterceptor interceptor) =>
            interceptor?.GetType() ?? throw new ArgumentNullException(nameof(interceptor));

        private void InsertInterceptor(int index, Type interceptorType, IJSInterceptor? interceptor)
        {
            JSInterceptorDescriptor interceptorDescriptor;

            if (interceptor is null) {
                interceptorDescriptor = new JSInterceptorDescriptor(currentRegistrationPhase, interceptorType);
            } else {
                interceptorDescriptor = new JSInterceptorDescriptor(currentRegistrationPhase, interceptor, interceptorType);
            }

            interceptorDescriptors.Insert(index, interceptorDescriptor);
        }

        public JSInterceptorBuilder Insert(int index, Type interceptorType)
        {
            InsertInterceptor(index, interceptorType, interceptor: null);
            return this;
        }

        public JSInterceptorBuilder Insert(int index, IJSInterceptor interceptor)
        {
            var interceptorType = GetInterceptorType(interceptor);
            InsertInterceptor(index, interceptorType, interceptor);
            return this;
        }

        public JSInterceptorBuilder Add(Type interceptorType) =>
            Insert(interceptorDescriptors.Count, interceptorType);

        public JSInterceptorBuilder Add(IJSInterceptor interceptor) =>
            Insert(interceptorDescriptors.Count, interceptor);

        public JSInterceptorBuilder Remove(Type interceptorType)
        {
            var interceptorDescriptor = new JSInterceptorDescriptor(currentRegistrationPhase, interceptorType);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSInterceptorBuilder Remove(IJSInterceptor interceptor)
        {
            var interceptorDescriptor = new JSInterceptorDescriptor(currentRegistrationPhase, interceptor, implementationType: null);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSInterceptorBuilder RemoveAt(int index)
        {
            interceptorDescriptors.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// Builds the interceptor.
        /// </summary>
        /// <param name="serviceProvider">
        /// If null the interceptor descriptors without implementation are
        /// skipped.
        /// </param>
        /// <param name="registrationPhaseServiceProvider">Provides service provider for each registration phase.</param>
        /// <returns></returns>
        public JSIterativeInterceptor Build(
            IServiceProvider? serviceProvider,
            IReadOnlyDictionary<InterceptorDescriptorRegistrationPhase, IServiceProvider?>? registrationPhaseServiceProvider = null)
        {
            var interceptors = new List<IJSInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                    continue;
                }

                var currentServiceProvider = registrationPhaseServiceProvider?.GetValueOrDefault(interceptorDescriptor.RegistrationPhase) ?? serviceProvider;

                if (currentServiceProvider is null) {
                    continue;
                }

                var interceptor = (IJSInterceptor)ActivatorUtilities.CreateInstance(currentServiceProvider, interceptorDescriptor.ImplementationType);
                interceptors.Add(interceptor);
            }

            return new JSIterativeInterceptor(interceptors);
        }

        #region IJSInterceptorBuilder

        IJSInterceptorBuilder IJSInterceptorBuilder.Insert(int index, Type interceptorType) =>
            Insert(index, interceptorType);

        IJSInterceptorBuilder IJSInterceptorBuilder.Insert(int index, IJSInterceptor interceptor) =>
            Insert(index, interceptor);

        IJSInterceptorBuilder IJSInterceptorBuilder.Add(Type interceptorType) =>
            Add(interceptorType);

        IJSInterceptorBuilder IJSInterceptorBuilder.Add(IJSInterceptor interceptor) =>
            Add(interceptor);

        IJSInterceptorBuilder IJSInterceptorBuilder.Remove(Type interceptorType) =>
            Remove(interceptorType);

        IJSInterceptorBuilder IJSInterceptorBuilder.Remove(IJSInterceptor interceptor) =>
            Remove(interceptor);

        IJSInterceptorBuilder IJSInterceptorBuilder.RemoveAt(int index) =>
            RemoveAt(index);

        #endregion
    }
}
