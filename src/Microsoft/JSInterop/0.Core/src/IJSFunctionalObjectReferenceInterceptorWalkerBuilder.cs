using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop
{
    public interface IJSFunctionalObjectReferenceInterceptorWalkerBuilder
    {
        IReadOnlyList<IJSFunctionalObjectReferenceInterceptor> Interceptors { get; }
        IReadOnlySet<Type> InterceptorTypes { get; }

        IJSFunctionalObjectReferenceInterceptorWalkerBuilder InsertInterceptor(int index, IJSFunctionalObjectReferenceInterceptor interceptor);
        IJSFunctionalObjectReferenceInterceptorWalkerBuilder AddInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor);
        IJSFunctionalObjectReferenceInterceptorWalkerBuilder RemoveInterceptor(IJSFunctionalObjectReferenceInterceptor interceptor);
    }
}
