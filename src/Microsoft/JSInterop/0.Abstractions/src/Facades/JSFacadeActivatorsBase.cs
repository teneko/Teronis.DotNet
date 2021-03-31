// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{
    /// <summary>
    /// A base class for assisting in proper usage of <see cref="IJSFacadeActivators"/>.
    /// </summary>
    public class JSFacadeActivatorsBase : IJSFacadeActivators
    {
        private List<IInstanceActivator<IAsyncDisposable>> instanceActivatorList;

        public JSFacadeActivatorsBase() =>
            instanceActivatorList = new List<IInstanceActivator<IAsyncDisposable>>();

        private void HookAnyInstanceActivated(InstanceActivatedDelegate<IAsyncDisposable> anyInstanceActivatedCallback)
        {
            foreach (var instanceActivator in instanceActivatorList) {
                instanceActivator.AnyInstanceActivated += anyInstanceActivatedCallback;
            }
        }

        /// <summary>
        /// Adds <paramref name="instanceActivator"/> to list. Must be called within constructor. List will be iterated later for 
        /// <see cref="IJSFacadeActivators.HookAnyInstanceActivated(InstanceActivatedDelegate{IAsyncDisposable})"/>
        /// </summary>
        /// <param name="instanceActivator"></param>
        protected void AddInstanceActivator(IInstanceActivator<IAsyncDisposable> instanceActivator) {
            instanceActivatorList.Add(instanceActivator);
        }

        /// <summary>
        /// Adds <paramref name="instanceActivators"/> to list. Must be called within constructor. List will be iterated later for 
        /// <see cref="IJSFacadeActivators.HookAnyInstanceActivated(InstanceActivatedDelegate{IAsyncDisposable})"/>
        /// </summary>
        /// <param name="instanceActivators"></param>
        protected void PrepareInstanceActivators(params IInstanceActivator<IAsyncDisposable>[] instanceActivators)
        {
            foreach (var instanceActivator in instanceActivators) {
                AddInstanceActivator(instanceActivator);
            }
        }

        void IJSFacadeActivators.HookAnyInstanceActivated(InstanceActivatedDelegate<IAsyncDisposable> registerAsyncDisposableFacadeHandler) =>
            HookAnyInstanceActivated(registerAsyncDisposableFacadeHandler);
    }
}
