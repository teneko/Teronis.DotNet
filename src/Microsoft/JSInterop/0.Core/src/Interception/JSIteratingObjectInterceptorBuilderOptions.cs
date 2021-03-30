// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSIteratingObjectInterceptorBuilderOptions<DerivedType>
        where DerivedType : JSIteratingObjectInterceptorBuilderOptions<DerivedType>
    {
        /// <summary>
        /// This instance will be used if it is not null. If it is not null the configuration made by 
        /// <see cref="ConfigureInterceptorBuilder(Action{IJSIteratingObjectInterceptorBuilder})"/>
        /// won't be taken into account.
        /// </summary>
        public IJSObjectInterceptor? Interceptor { get; set; }

        internal IServiceProvider ServiceProvider {
            set => serviceProvider = value;
        }

        public GetOrBuildInterceptorDelegate GetOrBuildInterceptorMethod { get; }

        protected JSIteratingObjectInterceptorBuilder InterceptorBuilder { get; }

        private IServiceProvider? serviceProvider;

        public JSIteratingObjectInterceptorBuilderOptions()
        {
            InterceptorBuilder = new JSIteratingObjectInterceptorBuilder();
            GetOrBuildInterceptorMethod = GetOrBuildInterceptor;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSIteratingObjectInterceptorBuilder"/>
        /// to create an implementation of <see cref="IJSObjectInterceptor"/> for being used as 
        /// <see cref="Interceptor"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorBuilder(Action<IJSIteratingObjectInterceptorBuilder> configure)
        {
            configure?.Invoke(InterceptorBuilder);
            return (DerivedType)this;
        }

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        private IJSObjectInterceptor GetOrBuildInterceptor(
            Action<IJSIteratingObjectInterceptorBuilder>? configureInterceptorWalkerBuilder)
        {
            if (configureInterceptorWalkerBuilder is null) {
                return Interceptor ??= InterceptorBuilder.BuildInterceptor(GetServiceProvider());
            }

            var additiveInterceptorWalkerBuilder = new JSIteratingObjectInterceptorBuilder(InterceptorBuilder.InterceptorDescriptors);
            configureInterceptorWalkerBuilder(additiveInterceptorWalkerBuilder);
            return additiveInterceptorWalkerBuilder.BuildInterceptor(GetServiceProvider());
        }
    }
}
