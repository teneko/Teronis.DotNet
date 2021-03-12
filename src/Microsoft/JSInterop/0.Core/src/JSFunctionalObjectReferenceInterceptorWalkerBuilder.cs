using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop
{
    public sealed class JSFunctionalObjectReferenceInterceptorWalkerBuilder : IJSFunctionalObjectReferenceInterceptorWalkerBuilder
    {
        public IReadOnlyList<IJSFunctionalObjectReferenceInterceptor> Interceptors =>
            interceptors;

        public IReadOnlySet<Type> InterceptorTypes =>
            interceptorTypes;

        private List<IJSFunctionalObjectReferenceInterceptor> interceptors;
        private HashSet<Type> interceptorTypes;

        public JSFunctionalObjectReferenceInterceptorWalkerBuilder()
        {
            interceptors = new List<IJSFunctionalObjectReferenceInterceptor>();
            interceptorTypes = new HashSet<Type>();
        }

        public JSFunctionalObjectReferenceInterceptorWalkerBuilder InsertInterceptor(int index, IJSFunctionalObjectReferenceInterceptor interceptor)
        {
            var interceptionType = interceptor?.GetType()
                ?? throw new ArgumentNullException(nameof(interceptor));

            if (interceptorTypes.Contains(interceptionType)) {
                throw new ArgumentException("The interception has been already added.");
            }

            interceptorTypes.Add(interceptionType);
            interceptors.Add(interceptor);
            return this;
        }

        public JSFunctionalObjectReferenceInterceptorWalkerBuilder AddInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor) =>
            InsertInterceptor(interceptors.Count, interceptor);

        public JSFunctionalObjectReferenceInterceptorWalkerBuilder RemoveInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor)
        {
            if (!(interceptor is null) && interceptors.Remove(interceptor)) {
                interceptorTypes.Remove(interceptor.GetType());
            }

            return this;
        }

        public JSFunctionalObjectReferenceInterceptorWalker BuildInterceptorWalker() =>
            new JSFunctionalObjectReferenceInterceptorWalker(interceptors);

        public JSInterceptableFunctionalObjectReference BuildInterceptableFunctionalObjectReference() {
            var interceptorWalker = BuildInterceptorWalker();
            return new JSInterceptableFunctionalObjectReference(interceptorWalker);
        }

        IJSFunctionalObjectReferenceInterceptorWalkerBuilder IJSFunctionalObjectReferenceInterceptorWalkerBuilder.InsertInterceptor(int index, IJSFunctionalObjectReferenceInterceptor interceptor) =>
            InsertInterceptor(index, interceptor);

        IJSFunctionalObjectReferenceInterceptorWalkerBuilder IJSFunctionalObjectReferenceInterceptorWalkerBuilder.AddInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor) =>
            AddInterceptor(interceptor);

        IJSFunctionalObjectReferenceInterceptorWalkerBuilder IJSFunctionalObjectReferenceInterceptorWalkerBuilder.RemoveInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor) =>
            RemoveInterceptor(interceptor);
    }
}
