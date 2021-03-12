using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectInterceptorWalkerBuilder
    {
        IReadOnlyList<JSFunctionalObjectInterceptorDescriptor> InterceptorDescritors { get; }

        IJSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, IJSFunctionalObjectInterceptor interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(IJSFunctionalObjectInterceptor interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(IJSFunctionalObjectInterceptor interceptor);
    }
}
