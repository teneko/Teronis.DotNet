using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSFunctionalObjectInterceptorWalkerBuilder : IJSFunctionalObjectInterceptorWalkerBuilder
    {
        public IReadOnlyList<JSFunctionalObjectInterceptorDescriptor> InterceptorDescriptors =>
            interceptorDescriptors;

        private List<JSFunctionalObjectInterceptorDescriptor> interceptorDescriptors;

        public JSFunctionalObjectInterceptorWalkerBuilder() =>
            interceptorDescriptors = new List<JSFunctionalObjectInterceptorDescriptor>();

        private Type GetInterceptorType(IJSFunctionalObjectInterceptor interceptor) =>
            interceptor?.GetType() ?? throw new ArgumentNullException(nameof(interceptor));

        private void InsertInterceptor(int index, Type interceptorType, IJSFunctionalObjectInterceptor? interceptor)
        {
            JSFunctionalObjectInterceptorDescriptor interceptorDescriptor;

            if (interceptor is null) {
                interceptorDescriptor = new JSFunctionalObjectInterceptorDescriptor(interceptorType);
            } else {
                interceptorDescriptor = new JSFunctionalObjectInterceptorDescriptor(interceptor, interceptorType);
            }

            interceptorDescriptors.Insert(index, interceptorDescriptor);
        }

        public JSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, Type interceptorType)
        {
            InsertInterceptor(index, interceptorType, interceptor: null);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, IJSFunctionalObjectInterceptor interceptor)
        {
            var interceptorType = GetInterceptorType(interceptor);
            InsertInterceptor(index, interceptorType, interceptor);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(Type interceptorType) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptorType);

        public JSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(IJSFunctionalObjectInterceptor interceptor) =>
            InsertInterceptor(interceptorDescriptors.Count, interceptor);

        public JSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(Type interceptorType)
        {
            var interceptorDescriptor = new JSFunctionalObjectInterceptorDescriptor(interceptorType);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        public JSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(IJSFunctionalObjectInterceptor interceptor)
        {
            var interceptorDescriptor = new JSFunctionalObjectInterceptorDescriptor(interceptor, implementationType: null);
            interceptorDescriptors.Remove(interceptorDescriptor);
            return this;
        }

        /// <summary>
        /// Builds an interceptor walker but skips those interceptor descriptors who have no implementation.
        /// </summary>
        /// <returns></returns>
        public JSFunctionalObjectInterceptorWalker BuildInterceptorWalker()
        {
            var interceptors = new List<IJSFunctionalObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                }
            }

            return new JSFunctionalObjectInterceptorWalker(interceptors);
        }

        public JSFunctionalObjectInterceptorWalker BuildInterceptorWalker(IServiceProvider serviceProvider)
        {
            var interceptors = new List<IJSFunctionalObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescriptors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                } else {
                    var interceptor = ActivatorUtilities.CreateInstance<IJSFunctionalObjectInterceptor>(serviceProvider, interceptorDescriptor.ImplementationType);
                    interceptors.Add(interceptor);
                }
            }

            return new JSFunctionalObjectInterceptorWalker(interceptors);
        }

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.InsertInterceptor(int index, Type interceptorType) =>
            InsertInterceptor(index, interceptorType);

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.InsertInterceptor(int index, IJSFunctionalObjectInterceptor interceptor) =>
            InsertInterceptor(index, interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.AddInterceptor(Type interceptorType) =>
            AddInterceptor(interceptorType);

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.AddInterceptor(IJSFunctionalObjectInterceptor interceptor) =>
            AddInterceptor(interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.RemoveInterceptor(Type interceptorType) =>
            RemoveInterceptor(interceptorType);

        IJSFunctionalObjectInterceptorWalkerBuilder IJSFunctionalObjectInterceptorWalkerBuilder.RemoveInterceptor(IJSFunctionalObjectInterceptor interceptor) =>
            RemoveInterceptor(interceptor);
    }
}
