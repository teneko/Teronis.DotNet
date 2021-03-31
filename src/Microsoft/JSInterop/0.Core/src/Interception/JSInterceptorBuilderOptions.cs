// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptorBuilderOptions<DerivedType>
        where DerivedType : JSInterceptorBuilderOptions<DerivedType>
    {
        internal virtual JSInterceptorBuilder InterceptorBuilder {
            get {
                if (interceptorBuilder is null) {
                    interceptorBuilder = new JSInterceptorBuilder();
                    isInterceptorBuilderUserTouched = true;
                }

                return interceptorBuilder;
            }
        }

        private IServiceProvider? serviceProvider;
        private JSInterceptorBuilder? interceptorBuilder;
        private IJSObjectInterceptor? interceptor;
        private bool isInterceptorBuilderUserTouched;

        internal IServiceProvider SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider;

        internal bool IsInterceptorBuilderUserUntouched() {
            if (isInterceptorBuilderUserTouched) {
                return false;
            }

            if (interceptorBuilder is null) {
                interceptorBuilder = new JSInterceptorBuilder();
            }

            return true;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSIteratingInterceptorBuilder"/>
        /// to create an implementation of <see cref="IJSObjectInterceptor"/> for being used as 
        /// <see cref="interceptor"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorBuilder(Action<IJSIteratingInterceptorBuilder> configure)
        {
            configure?.Invoke(InterceptorBuilder);
            return (DerivedType)this;
        }

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        public IJSObjectInterceptor BuildInterceptor(
            Action<IJSIteratingInterceptorBuilder>? configureBuilder)
        {
            if (configureBuilder is null && !(interceptor is null)) {
                return interceptor;
            }

            var serviceProvider = GetServiceProvider();

            if (configureBuilder is null) {
                return InterceptorBuilder.Build(serviceProvider);
            }

            var mutatingInterceptorBuilder = new JSInterceptorBuilder(InterceptorBuilder.InterceptorDescriptors);
            mutatingInterceptorBuilder.SetRegistrationPhase(InterceptorDescriptorRegistrationPhase.FacadeActivation);
            configureBuilder(mutatingInterceptorBuilder);
            return mutatingInterceptorBuilder.Build(serviceProvider);
        }
    }
}
