using System;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectOptions<DerivedType>
        where DerivedType : JSFunctionalObjectOptions<DerivedType>
    {
        /// <summary>
        /// This instance will be used if it is not null. If it is not null the configuration made by 
        /// <see cref="ConfigureInterceptorWalkerBuilder(Action{IJSFunctionalObjectInterceptorWalkerBuilder})"/>
        /// won't be taken into account.
        /// </summary>
        public IJSFunctionalObject? JSFunctionalObject { get; set; }

        internal IServiceProvider ServiceProvider {
            set => serviceProvider = value;
        }

        public GetOrBuildJSFunctionalObjectDelegate GetOrBuildJSFunctionalObject { get; }

        protected JSFunctionalObjectInterceptorWalkerBuilder InterceptorWalkerBuilder { get; }

        private IServiceProvider? serviceProvider;

        public JSFunctionalObjectOptions()
        {
            InterceptorWalkerBuilder = new JSFunctionalObjectInterceptorWalkerBuilder();
            GetOrBuildJSFunctionalObject = GetOrBuildJSFunctionalObject_Implementation;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSFunctionalObjectInterceptorWalkerBuilder"/>
        /// to create an implementation of <see cref="IJSFunctionalObject"/> for being used as 
        /// <see cref="JSFunctionalObject"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public DerivedType ConfigureInterceptorWalkerBuilder(Action<IJSFunctionalObjectInterceptorWalkerBuilder> configure)
        {
            configure?.Invoke(InterceptorWalkerBuilder);
            return (DerivedType)this;
        }

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        private IJSFunctionalObject GetOrBuildJSFunctionalObject_Implementation() =>
            JSFunctionalObject ??= InterceptorWalkerBuilder.BuildInterceptableFunctionalObject(GetServiceProvider());
    }
}
