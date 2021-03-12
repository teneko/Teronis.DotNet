using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop
{
    public sealed class JSFunctionalObjectInterceptorWalkerBuilder : IJSFunctionalObjectInterceptorWalkerBuilder
    {
        public IReadOnlyList<JSFunctionalObjectInterceptorDescriptor> InterceptorDescritors =>
            interceptorDescriptors;

        private List<JSFunctionalObjectInterceptorDescriptor> interceptorDescriptors;

        public JSFunctionalObjectInterceptorWalkerBuilder() =>
            interceptorDescriptors = new List<JSFunctionalObjectInterceptorDescriptor>();

        private void AddInterceptor(JSFunctionalObjectInterceptorDescriptor interceptorDescriptor)
        {
            if (interceptorDescriptors.Contains(interceptorDescriptor)) {
                throw new ArgumentException("The interception has been already added.");
            }

            interceptorDescriptors.Add(interceptorDescriptor);
        }

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

            AddInterceptor(interceptorDescriptor);
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
            var interceptorType = GetInterceptorType(interceptor);
            return RemoveInterceptor(interceptorType);
        }

        public JSFunctionalObjectInterceptorWalker BuildInterceptorWalker(IServiceProvider serviceProvider) {
            var interceptors = new List<IJSFunctionalObjectInterceptor>();

            foreach (var interceptorDescriptor in InterceptorDescritors) {
                if (interceptorDescriptor.HasImplementation) {
                    interceptors.Add(interceptorDescriptor.Implementation!);
                } else {
                    var interceptor = ActivatorUtilities.CreateInstance<IJSFunctionalObjectInterceptor>(serviceProvider, interceptorDescriptor.ImplementationType);
                    interceptors.Add(interceptor);
                }
            }

            return new JSFunctionalObjectInterceptorWalker(interceptors);
        }

        public JSInterceptableFunctionalObject BuildInterceptableFunctionalObject(IServiceProvider serviceProvider)
        {
            var interceptorWalker = BuildInterceptorWalker(serviceProvider);
            return new JSInterceptableFunctionalObject(interceptorWalker);
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
