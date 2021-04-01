// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSInterceptorBuilderOptions<DerivedType> : ILateInterceptorBuilder 
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
        private IJSInterceptor? interceptor;
        private bool isInterceptorBuilderUserTouched;

        internal IServiceProvider SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider;

        internal bool TryCreateInterceptorBuilderUserUntouched()
        {
            if (isInterceptorBuilderUserTouched) {
                return false;
            }

            if (interceptorBuilder is null) {
                interceptorBuilder = new JSInterceptorBuilder();
            }

            return true;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSInterceptorBuilder"/>
        /// to create an implementation of <see cref="IJSInterceptor"/> for being used as 
        /// <see cref="interceptor"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorBuilder(Action<IJSInterceptorBuilder> configure)
        {
            configure?.Invoke(InterceptorBuilder);
            return (DerivedType)this;
        }

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        public IJSInterceptor BuildInterceptor(
            Action<IJSInterceptorBuilder>? configureBuilder)
        {
            if (configureBuilder is null && !(interceptor is null)) {
                return interceptor;
            }

            var serviceProvider = GetServiceProvider();

            if (configureBuilder is null) {
                return interceptor = InterceptorBuilder.Build(serviceProvider);
            }

            var mutatingInterceptorBuilder = new JSInterceptorBuilder(InterceptorBuilder.InterceptorDescriptors);
            mutatingInterceptorBuilder.SetRegistrationPhase(InterceptorDescriptorRegistrationPhase.FacadeActivation);
            configureBuilder(mutatingInterceptorBuilder);
            return mutatingInterceptorBuilder.Build(serviceProvider);
        }
    }
}
