using System;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    public static class IJSDynamicProxyActivatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type <typeparamref name="T"/> has to be an interface.</typeparam>
        /// <param name="jsObjectReference">An original reference of <see cref="IJSRuntime"/>.</param>
        /// <param name="jsObjectFacadeToBeProxied">A facade or proxy that implements <see cref="IJSObjectReferenceFacade"/>.</param>
        /// <param name="jsFunctionalObject">The functional object of <paramref name="jsObjectReference"/>.</param>
        /// <param name="creationOptions">The dynamic proxy creation options.</param>
        /// <returns>A proxy that implements <typeparamref name="T"/>.</returns>
        /// <exception cref="NotSupportedException">Only interface type is allowed to be proxied.</exception>
        public static T CreateInstance<T>(
            this IJSDynamicProxyActivator jsDynamicProxyActivator,
            IJSObjectReferenceFacade jsObjectFacadeToBeProxied,
            IJSFunctionalObject? jsFunctionalObject = null,
            DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade =>
            (T)jsDynamicProxyActivator.CreateInstance(
                interfaceToBeProxied: typeof(T),
                jsObjectFacadeToBeProxied,
                jsFunctionalObject,
                creationOptions);

        public static T CreateInstance<T>(
            this IJSDynamicProxyActivator jsDynamicProxyActivator,
            IJSObjectReference jsObjectReference,
            DynamicProxyCreationOptions? creationOptions = null)
            where T : class, IJSObjectReferenceFacade =>
            (T)jsDynamicProxyActivator.CreateInstance(
                interfaceToBeProxied: typeof(T),
                jsObjectReference,
                creationOptions);
    }
}
