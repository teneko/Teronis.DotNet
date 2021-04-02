// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Interception.Interceptor;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class IJSDynamicProxyActivatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type <typeparamref name="T"/> has to be an interface.</typeparam>
        /// <param name="jsDynamicProxyActivator">The JavaScript dynamic object proxy activator.</param>
        /// <param name="jsObjectFacadeToBeProxied">A facade or proxy that implements <see cref="IJSObjectReferenceFacade"/>.</param>
        /// <param name="jsInterceptor">The functional object of <paramref name="jsObjectFacadeToBeProxied"/>.</param>
        /// <param name="creationOptions">The dynamic proxy creation options.</param>
        /// <returns>A proxy that implements <typeparamref name="T"/>.</returns>
        /// <exception cref="NotSupportedException">Only interface type is allowed to be proxied.</exception>
        public static T CreateInstance<T>(
            this IJSDynamicProxyActivator jsDynamicProxyActivator,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSInterceptor? jsInterceptor = null,
            DynamicProxyCreationOptions? creationOptions = null)
            where T : class =>
            (T)jsDynamicProxyActivator.CreateInstance(
                interfaceToBeProxied: typeof(T),
                jsObjectFacadeToBeProxied,
                jsInterceptor,
                creationOptions);

        public static T CreateInstance<T>(
            this IJSDynamicProxyActivator jsDynamicProxyActivator,
            IJSObjectReference jsObjectReference,
            DynamicProxyCreationOptions? creationOptions = null)
            where T : class =>
            (T)jsDynamicProxyActivator.CreateInstance(
                interfaceToBeProxied: typeof(T),
                jsObjectReference,
                creationOptions);
    }
}
