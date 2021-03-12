using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop
{
    //public static class IServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddJSInterceptableObjectReference(
    //        this IServiceCollection services,
    //        Action<IJSFunctionalObjectReferenceInterceptorWalkerBuilder>? configureInterceptionWalkerBuilder = null)
    //    {
    //        var interceptionWalkerBuilder = new JSFunctionalObjectReferenceInterceptorWalkerBuilder();
    //        configureInterceptionWalkerBuilder?.Invoke(interceptionWalkerBuilder);
    //        var interceptionWalker = interceptionWalkerBuilder.Build();
    //        var interceptableFunctionalObjectReference = new JSInterceptableFunctionalObjectReference(interceptionWalker);
    //        services.AddSingleton<IJSFunctionalObjectReference>(interceptableFunctionalObjectReference);
    //        return services;
    //    }
    //}
}
