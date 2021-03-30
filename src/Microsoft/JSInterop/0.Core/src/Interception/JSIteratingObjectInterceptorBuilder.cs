// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSIteratingObjectInterceptorBuilder : IJSIteratingObjectInterceptorBuilder
    {
        public IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors =>
            interceptorDescriptors;

        private List<JSObjectInterceptorDescriptor> interceptorDescriptors;

        public JSIteratingObjectInterceptorBuilder() =>
            interceptorDescriptors = new List<JSObjectInterceptorDescriptor>();

        public JSIteratingObjectInterceptorBuilder(IEnumerable<JSObjectInterceptorDescriptor> interceptorDescriptors) =>
            this.interceptorDescriptors = new List<JSObjectInterceptorDescriptor>(interceptorDescriptors);

        private Type GetInterceptorType(IJSObjectInterceptor interceptor) =>
            interceptor?.GetType() ?? throw new ArgumentNullException(nameof(interceptor));

        private void InsertInterceptor(int index, Type interceptorType, IJSObjectInterceptor? interceptor)
        {
            JSObjectInterceptorDescriptor interceptorDescriptor;

            if (interceptor is null) {
                interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptorType);
            } else {
                interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptor, interceptorType);
            }

            interceptorDescriptors.Insert(index, interceptorDescriptor);
        }

        public JSIteratingObjectInterceptorBuilder InsertInterceptor(int index, Type interceptorType)
        {
            InsertInterceptor(index, interceptorType, interceptor: null);
            return this;
        }

        public JSIteratingObjectInterceptorBuilder InsertInterceptor(int index, IJSObjectInterceptor interceptor)
        {
            var interceptorType = GetInterceptorType(interceptor);
            InsertInterceptor(index, interceptorType, interceptor);
            return this;
        }

        public JSIteratingObjectInterceptorBuilder AddInterceptor(Type interceptorType) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptorType);

        public JSIteratingObjectInterceptorBuilder AddInterceptor(IJSObjectInterceptor interceptor) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptor);

        public JSIteratingObjectInterceptorBuilder RemoveInterceptor(Type interceptorType)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptorType);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSIteratingObjectInterceptorBuilder RemoveInterceptor(IJSObjectInterceptor interceptor)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptor, implementationType: null);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        /// <summary>
        /// Builds an interceptor walker but skips those interceptor descriptors who have no implementation.
        /// </summary>
        /// <returns></returns>
        public JSIteratingObjectInterceptor BuildInterceptor()
        {
            var interceptors = new List<IJSObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                }
            }

            return new JSIteratingObjectInterceptor(interceptors);
        }

        public JSIteratingObjectInterceptor BuildInterceptor(IServiceProvider serviceProvider)
        {
            var interceptors = new List<IJSObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                } else {
                    var interceptor = (IJSObjectInterceptor)ActivatorUtilities.CreateInstance(serviceProvider, interceptorDescriptor.ImplementationType);
                    interceptors.Add(interceptor);
                }
            }

            return new JSIteratingObjectInterceptor(interceptors);
        }

        #region IJSObjectInterceptorWalkerBuilder

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.InsertInterceptor(int index, Type interceptorType) =>
            InsertInterceptor(index, interceptorType);

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.InsertInterceptor(int index, IJSObjectInterceptor interceptor) =>
            InsertInterceptor(index, interceptor);

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.AddInterceptor(Type interceptorType) =>
            AddInterceptor(interceptorType);

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.AddInterceptor(IJSObjectInterceptor interceptor) =>
            AddInterceptor(interceptor);

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.RemoveInterceptor(Type interceptorType) =>
            RemoveInterceptor(interceptorType);

        IJSIteratingObjectInterceptorBuilder IJSIteratingObjectInterceptorBuilder.RemoveInterceptor(IJSObjectInterceptor interceptor) =>
            RemoveInterceptor(interceptor);

        #endregion
    }
}
