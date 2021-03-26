// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeActivators
    {
        /// <summary>
        /// A hook called in constructor of class that implements <see cref="IJSFacades{TJSFacadeActivators}"/>. Expects all
        /// member that implement <see cref="IInstanceActivator{T}"/> to add <paramref name="registerAsyncDisposableFacade"/>
        /// to <see cref="IInstanceActivator{T}.InstanceActivated"/>.
        /// </summary>
        /// <param name="registerAsyncDisposableFacade"></param>
        void PrepareInstanceActivatedCapableActivators(InstanceActivatedDelegate<IAsyncDisposable> registerAsyncDisposableFacade);
    }
}
