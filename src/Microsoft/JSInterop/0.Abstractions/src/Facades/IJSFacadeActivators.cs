// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeActivators
    {
        /// <summary>
        /// A hook called in constructor of class that implements <see cref="IJSFacadeHub{TJSFacadeActivators}"/>. This method injects
        /// the delegate that can be called in derived classes of <see cref="IJSFacadeActivators"/>. It is expected that every activated
        /// instance of an activator (implements most of times <see cref="IInstanceActivator{T}"/>) is passed to
        /// <paramref name="registerAsyncDisposableFacade"/>.
        /// </summary>
        /// <param name="registerAsyncDisposableFacade">The method that registers an disposable instance to be disposed.</param>
        void HookAnyInstanceActivated(InstanceActivatedDelegate<IAsyncDisposable> registerAsyncDisposableFacade);
    }
}
