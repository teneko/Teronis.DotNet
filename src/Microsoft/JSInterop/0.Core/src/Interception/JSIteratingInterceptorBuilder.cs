// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal sealed class JSIteratingInterceptorBuilder : IJSIteratingInterceptorBuilder
    {
        public IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors =>
            interceptorDescriptors;

        private List<JSObjectInterceptorDescriptor> interceptorDescriptors;
        private InterceptorDescriptorRegistrationPhase currentRegistrationPhase;

        public JSIteratingInterceptorBuilder() =>
            interceptorDescriptors = new List<JSObjectInterceptorDescriptor>();

        public JSIteratingInterceptorBuilder(IEnumerable<JSObjectInterceptorDescriptor> interceptorDescriptors) =>
            this.interceptorDescriptors = new List<JSObjectInterceptorDescriptor>(interceptorDescriptors);

        /// <summary>
        /// Sets the current registration phase.
        /// </summary>
        /// <param name="registrationPhase"></param>
        public void SetRegistrationPhase(InterceptorDescriptorRegistrationPhase registrationPhase) =>
            currentRegistrationPhase = registrationPhase;

        private Type GetInterceptorType(IJSObjectInterceptor interceptor) =>
            interceptor?.GetType() ?? throw new ArgumentNullException(nameof(interceptor));

        private void InsertInterceptor(int index, Type interceptorType, IJSObjectInterceptor? interceptor)
        {
            JSObjectInterceptorDescriptor interceptorDescriptor;

            if (interceptor is null) {
                interceptorDescriptor = new JSObjectInterceptorDescriptor(currentRegistrationPhase, interceptorType);
            } else {
                interceptorDescriptor = new JSObjectInterceptorDescriptor(currentRegistrationPhase, interceptor, interceptorType);
            }

            interceptorDescriptors.Insert(index, interceptorDescriptor);
        }

        public JSIteratingInterceptorBuilder Insert(int index, Type interceptorType)
        {
            InsertInterceptor(index, interceptorType, interceptor: null);
            return this;
        }

        public JSIteratingInterceptorBuilder Insert(int index, IJSObjectInterceptor interceptor)
        {
            var interceptorType = GetInterceptorType(interceptor);
            InsertInterceptor(index, interceptorType, interceptor);
            return this;
        }

        public JSIteratingInterceptorBuilder Add(Type interceptorType) =>
            Insert(interceptorDescriptors.Count, interceptorType);

        public JSIteratingInterceptorBuilder Add(IJSObjectInterceptor interceptor) =>
            Insert(interceptorDescriptors.Count, interceptor);

        public JSIteratingInterceptorBuilder Remove(Type interceptorType)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(currentRegistrationPhase, interceptorType);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSIteratingInterceptorBuilder Remove(IJSObjectInterceptor interceptor)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(currentRegistrationPhase, interceptor, implementationType: null);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSIteratingInterceptorBuilder RemoveAt(int index)
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
        public JSIteratingInterceptor Build(
            IServiceProvider? serviceProvider,
            IReadOnlyDictionary<InterceptorDescriptorRegistrationPhase, IServiceProvider?>? registrationPhaseServiceProvider = null)
        {
            var interceptors = new List<IJSObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                    continue;
                }

                var currentServiceProvider = registrationPhaseServiceProvider?.GetValueOrDefault(interceptorDescriptor.RegistrationPhase) ?? serviceProvider;

                if (currentServiceProvider is null) {
                    continue;
                }

                var interceptor = (IJSObjectInterceptor)ActivatorUtilities.CreateInstance(currentServiceProvider, interceptorDescriptor.ImplementationType);
                interceptors.Add(interceptor);
            }

            return new JSIteratingInterceptor(interceptors);
        }

        #region IJSObjectInterceptorWalkerBuilder

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Insert(int index, Type interceptorType) =>
            Insert(index, interceptorType);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Insert(int index, IJSObjectInterceptor interceptor) =>
            Insert(index, interceptor);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Add(Type interceptorType) =>
            Add(interceptorType);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Add(IJSObjectInterceptor interceptor) =>
            Add(interceptor);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Remove(Type interceptorType) =>
            Remove(interceptorType);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.Remove(IJSObjectInterceptor interceptor) =>
            Remove(interceptor);

        IJSIteratingInterceptorBuilder IJSIteratingInterceptorBuilder.RemoveAt(int index) =>
            RemoveAt(index);

        #endregion
    }
}
