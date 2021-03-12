using System;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectReferenceOptions
    {
        /// <summary>
        /// This instance will be used if it is not null. If it is not null the configuration made by 
        /// <see cref="ConfigureInterceptorWalkerBuilder(Action{IJSFunctionalObjectReferenceInterceptorWalkerBuilder})"/>
        /// won't be taken into account.
        /// </summary>
        public IJSFunctionalObjectReference? JSFunctionalObjectReference { get; set; }

        private JSFunctionalObjectReferenceInterceptorWalkerBuilder jsInterceptableObjectReferenceBuilder;

        public JSFunctionalObjectReferenceOptions() =>
            jsInterceptableObjectReferenceBuilder = new JSFunctionalObjectReferenceInterceptorWalkerBuilder();

        /// <summary>
        /// Configures an implementation of <see cref="IJSFunctionalObjectReferenceInterceptorWalkerBuilder"/>
        /// to create an implementation of <see cref="IJSFunctionalObjectReference"/> for being used as 
        /// <see cref="JSFunctionalObjectReference"/> when it is null.
        /// </summary>
        /// <param name="configure"></param>
        public void ConfigureInterceptorWalkerBuilder(Action<IJSFunctionalObjectReferenceInterceptorWalkerBuilder> configure) =>
            configure?.Invoke(jsInterceptableObjectReferenceBuilder);

        internal IJSFunctionalObjectReference GetOrBuildJSFunctionalObjectReference() =>
            JSFunctionalObjectReference ?? jsInterceptableObjectReferenceBuilder.BuildInterceptableFunctionalObjectReference();
    }
}
