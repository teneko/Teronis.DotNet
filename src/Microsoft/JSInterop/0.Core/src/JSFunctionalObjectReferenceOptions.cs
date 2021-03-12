using System;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectOptions
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

        internal GetOrBuildJSFunctionalObjectDelegate GetOrBuildJSFunctionalObjectDelegate { get; }

        protected JSFunctionalObjectInterceptorWalkerBuilder InterceptorWalkerBuilder { get; }

        private IServiceProvider? serviceProvider;

        public JSFunctionalObjectOptions()
        {
            InterceptorWalkerBuilder = new JSFunctionalObjectInterceptorWalkerBuilder();
            GetOrBuildJSFunctionalObjectDelegate = GetOrBuildJSFunctionalObject;
        }

        /// <summary>
        /// Configures an implementation of <see cref="IJSFunctionalObjectInterceptorWalkerBuilder"/>
        /// to create an implementation of <see cref="IJSFunctionalObject"/> for being used as 
        /// <see cref="JSFunctionalObject"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public void ConfigureInterceptorWalkerBuilder(Action<IJSFunctionalObjectInterceptorWalkerBuilder> configure) =>
            configure?.Invoke(InterceptorWalkerBuilder);

        private IServiceProvider GetServiceProvider() =>
            serviceProvider ?? throw new InvalidOperationException("Service provider has not been set.");

        private IJSFunctionalObject GetOrBuildJSFunctionalObject() =>
            JSFunctionalObject ??= InterceptorWalkerBuilder.BuildInterceptableFunctionalObject(GetServiceProvider());
    }
}
