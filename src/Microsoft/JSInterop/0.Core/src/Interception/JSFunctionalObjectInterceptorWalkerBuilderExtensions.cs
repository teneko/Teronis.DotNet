using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class JSFunctionalObjectInterceptorWalkerBuilderExtensions
    {
        /// <summary>
        /// Builds an interceptable functional object but skips those interceptor descriptors who have no implementation.
        /// </summary>
        /// <returns></returns>
        public static JSInterceptableFunctionalObject BuildInterceptableFunctionalObject(
            this JSFunctionalObjectInterceptorWalkerBuilder interceptorWalkerBuilder)
        {
            var interceptorWalker = interceptorWalkerBuilder.BuildInterceptorWalker();
            return new JSInterceptableFunctionalObject(interceptorWalker);
        }

        public static JSInterceptableFunctionalObject BuildInterceptableFunctionalObject(
            this JSFunctionalObjectInterceptorWalkerBuilder interceptorWalkerBuilder,
            IServiceProvider serviceProvider)
        {
            var interceptorWalker = interceptorWalkerBuilder.BuildInterceptorWalker(serviceProvider);
            return new JSInterceptableFunctionalObject(interceptorWalker);
        }
    }
}
