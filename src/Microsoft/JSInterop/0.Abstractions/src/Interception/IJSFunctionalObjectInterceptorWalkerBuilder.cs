using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInterceptorWalkerBuilder
    {
        IReadOnlyList<JSFunctionalObjectInterceptorDescriptor> InterceptorDescriptors { get; }

        IJSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder InsertInterceptor(int index, IJSFunctionalObjectInterceptor interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder AddInterceptor(IJSFunctionalObjectInterceptor interceptor);

        IJSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(Type interceptorType);
        IJSFunctionalObjectInterceptorWalkerBuilder RemoveInterceptor(IJSFunctionalObjectInterceptor interceptor);
    }
}
