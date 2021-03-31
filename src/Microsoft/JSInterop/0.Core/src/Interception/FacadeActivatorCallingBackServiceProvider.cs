// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    internal class FacadeActivatorCallingBackServiceProvider : IServiceProvider, IAsyncDisposable
    {
        public IReadOnlyList<IInstanceActivator<IAsyncDisposable>> FacadeActivators =>
            facadeActivators;

        private readonly IServiceProvider serviceProvider;
        private readonly InstanceActivatedDelegate<IAsyncDisposable> facadeActivatedCallback;
        private readonly List<IInstanceActivator<IAsyncDisposable>> facadeActivators;

        public FacadeActivatorCallingBackServiceProvider(IServiceProvider serviceProvider, InstanceActivatedDelegate<IAsyncDisposable> facadeActivatedCallback)
        {
            facadeActivators = new List<IInstanceActivator<IAsyncDisposable>>();
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.facadeActivatedCallback = facadeActivatedCallback ?? throw new ArgumentNullException(nameof(facadeActivatedCallback));
        }

        public object? GetService(Type serviceType)
        {
            var service = serviceProvider.GetService(serviceType);

            if (service is IInstanceActivator<IAsyncDisposable> facadeActivator) {
                facadeActivator.AnyInstanceActivated += FacadeActivator_FacadeActivated;
            }

            return service;
        }

        private void FacadeActivator_FacadeActivated(IAsyncDisposable activatedInstance) =>
            facadeActivatedCallback(activatedInstance);

        public ValueTask DisposeAsync()
        {
            foreach (var facadeActivator in facadeActivators) {
                facadeActivator.AnyInstanceActivated -= FacadeActivator_FacadeActivated;
            }

            return ValueTask.CompletedTask;
        }
    }
}
