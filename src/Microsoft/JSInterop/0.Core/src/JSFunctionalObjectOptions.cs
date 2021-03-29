// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectOptions<DerivedType>
        where DerivedType : JSFunctionalObjectOptions<DerivedType>
    {
        /// <summary>
        /// This instance will be used if it is not null. If it is not null the configuration made by 
        /// <see cref="ConfigureInterceptorWalkerBuilder(Action{IJSObjectInterceptorWalkerBuilder})"/>
        /// won't be taken into account.
        /// </summary>
        public IJSFunctionalObject? JSFunctionalObject { get; set; }

        internal IServiceProvider ServiceProvider {
            set => serviceProvider = value;
        }

        public GetOrBuildJSInterceptableFunctionalObjectDelegate GetOrBuildJSInterceptableFunctionalObject { get; }

        protected JSFunctionalObjectInterceptorWalkerBuilder InterceptorWalkerBuilder { get; }

        private IServiceProvider? serviceProvider;

        public JSFunctionalObjectOptions()
        {
            InterceptorWalkerBuilder = new JSFunctionalObjectInterceptorWalkerBuilder();
            GetOrBuildJSInterceptableFunctionalObject = GetOrBuildJSFunctionalObject_Implementation;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSObjectInterceptorWalkerBuilder"/>
        /// to create an implementation of <see cref="IJSFunctionalObject"/> for being used as 
        /// <see cref="JSFunctionalObject"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorWalkerBuilder(Action<IJSObjectInterceptorWalkerBuilder> configure)
        {
            configure?.Invoke(InterceptorWalkerBuilder);
            return (DerivedType)this;
        }

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        private IJSFunctionalObject GetOrBuildJSFunctionalObject_Implementation(
            Action<IJSObjectInterceptorWalkerBuilder>? configureInterceptorWalkerBuilder)
        {
            if (configureInterceptorWalkerBuilder is null) {
                return JSFunctionalObject ??= InterceptorWalkerBuilder.BuildInterceptableFunctionalObject(GetServiceProvider());
            }

            var additiveInterceptorWalkerBuilder = new JSFunctionalObjectInterceptorWalkerBuilder(InterceptorWalkerBuilder.InterceptorDescriptors);
            configureInterceptorWalkerBuilder(additiveInterceptorWalkerBuilder);
            return additiveInterceptorWalkerBuilder.BuildInterceptableFunctionalObject(GetServiceProvider());
        }
    }
}
