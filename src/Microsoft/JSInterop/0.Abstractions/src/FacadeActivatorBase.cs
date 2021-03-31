// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop
{
    public class FacadeActivatorBase<T> : IInstanceActivator<T>
        where T : IAsyncDisposable
    {
        public event InstanceActivatedDelegate<IAsyncDisposable>? AnyInstanceActivated;
        public event InstanceActivatedDelegate<T>? InstanceActivated;

        protected void DispatchAnyInstanceActivated(IAsyncDisposable anyActivatedFacade) =>
            AnyInstanceActivated?.Invoke(anyActivatedFacade);

        protected void DispatchFacadeActicated(T activatedFacade)
        {
            DispatchAnyInstanceActivated(activatedFacade);
            InstanceActivated?.Invoke(activatedFacade);
        }
    }
}
