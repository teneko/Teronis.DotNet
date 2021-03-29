// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSFunctionalObjectInterceptorWalkerBuilder : IJSObjectInterceptorWalkerBuilder
    {
        public IReadOnlyList<JSObjectInterceptorDescriptor> InterceptorDescriptors =>
            interceptorDescriptors;

        private List<JSObjectInterceptorDescriptor> interceptorDescriptors;

        public JSFunctionalObjectInterceptorWalkerBuilder() =>
            interceptorDescriptors = new List<JSObjectInterceptorDescriptor>();

        public JSFunctionalObjectInterceptorWalkerBuilder(IEnumerable<JSObjectInterceptorDescriptor> interceptorDescriptors) =>
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

        public JSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, Type interceptorType)
        {
            InsertInterceptor(index, interceptorType, interceptor: null);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, IJSObjectInterceptor interceptor)
        {
            var interceptorType = GetInterceptorType(interceptor);
            InsertInterceptor(index, interceptorType, interceptor);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(Type interceptorType) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptorType);

        public JSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(IJSObjectInterceptor interceptor) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptor);

        public JSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(Type interceptorType)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptorType);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(IJSObjectInterceptor interceptor)
        {
            var interceptorDescriptor = new JSObjectInterceptorDescriptor(interceptor, implementationType: null);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        /// <summary>
        /// Builds an interceptor walker but skips those interceptor descriptors who have no implementation.
        /// </summary>
        /// <returns></returns>
        public JSFunctionalObjectInterceptorWalker BuildInterceptorWalker()
        {
            var interceptors = new List<IJSObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                }
            }

            return new JSFunctionalObjectInterceptorWalker(interceptors);
        }

        public JSFunctionalObjectInterceptorWalker BuildInterceptorWalker(IServiceProvider serviceProvider)
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

            return new JSFunctionalObjectInterceptorWalker(interceptors);
        }

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.InsertInterceptor(int index, Type interceptorType) =>
            InsertInterceptor(index, interceptorType);

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.InsertInterceptor(int index, IJSObjectInterceptor interceptor) =>
            InsertInterceptor(index, interceptor);

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.AddInterceptor(Type interceptorType) =>
            AddInterceptor(interceptorType);

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.AddInterceptor(IJSObjectInterceptor interceptor) =>
            AddInterceptor(interceptor);

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.RemoveInterceptor(Type interceptorType) =>
            RemoveInterceptor(interceptorType);

        IJSObjectInterceptorWalkerBuilder IJSObjectInterceptorWalkerBuilder.RemoveInterceptor(IJSObjectInterceptor interceptor) =>
            RemoveInterceptor(interceptor);
    }
}
