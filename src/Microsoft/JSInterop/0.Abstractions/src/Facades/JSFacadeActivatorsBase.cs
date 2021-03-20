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

        private void PrepareInstanceActivatedCapableActivators(InstanceActivatedDelegate<IAsyncDisposable> registerAsyncDisposableFacadeHandler)
        {
            foreach (var instanceActivator in instanceActivatorList) {
                instanceActivator.InstanceActivated += registerAsyncDisposableFacadeHandler;
            }
        }

        /// <summary>
        /// Adds <paramref name="instanceActivator"/> to list. Must be called within constructor. List will be iterated later for 
        /// <see cref="IJSFacadeActivators.PrepareInstanceActivatedCapableActivators(InstanceActivatedDelegate{IAsyncDisposable})"/>
        /// </summary>
        /// <param name="instanceActivator"></param>
        protected void AddInstanceActivator(IInstanceActivator<IAsyncDisposable> instanceActivator) {
            instanceActivatorList.Add(instanceActivator);
        }

        /// <summary>
        /// Adds <paramref name="instanceActivator"/> to list. Must be called within constructor. List will be iterated later for 
        /// <see cref="IJSFacadeActivators.PrepareInstanceActivatedCapableActivators(InstanceActivatedDelegate{IAsyncDisposable})"/>
        /// </summary>
        /// <param name="instanceActivator"></param>
        protected void PrepareInstanceActivators(params IInstanceActivator<IAsyncDisposable>[] instanceActivators)
        {
            foreach (var instanceActivator in instanceActivators) {
                AddInstanceActivator(instanceActivator);
            }
        }

        void IJSFacadeActivators.PrepareInstanceActivatedCapableActivators(InstanceActivatedDelegate<IAsyncDisposable> registerAsyncDisposableFacadeHandler) =>
            PrepareInstanceActivatedCapableActivators(registerAsyncDisposableFacadeHandler);
    }
}
