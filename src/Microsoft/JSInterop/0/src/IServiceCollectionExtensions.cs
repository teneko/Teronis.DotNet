// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop
{
    //public static class IServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddJSInterceptableObjectReference(
    //        this IServiceCollection services,
    //        Action<IJSFunctionalObjectInterceptorWalkerBuilder>? configureInterceptionWalkerBuilder = null)
    //    {
    //        var interceptionWalkerBuilder = new JSFunctionalObjectInterceptorWalkerBuilder();
    //        configureInterceptionWalkerBuilder?.Invoke(interceptionWalkerBuilder);
    //        var interceptionWalker = interceptionWalkerBuilder.Build();
    //        var interceptableFunctionalObject = new JSInterceptableFunctionalObject(interceptionWalker);
    //        services.AddSingleton<IJSFunctionalObject>(interceptableFunctionalObject);
    //        return services;
    //    }
    //}
}
