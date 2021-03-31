// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeHub<TJSFacadeActivators> : IJSFacadeHub<TJSFacadeActivators>
        where TJSFacadeActivators : class
    {
        public IReadOnlyList<IAsyncDisposable> ComponentDisposables =>
            componentDisposables;

        public TJSFacadeActivators Activators { get; }

        private readonly List<IAsyncDisposable> componentDisposables;

        public JSFacadeHub(TJSFacadeActivators jsFacadeActivators)
        {
            componentDisposables = new List<IAsyncDisposable>();
            Activators = jsFacadeActivators ?? throw new ArgumentNullException(nameof(Activators));
        }

        public IAsyncDisposable this[int index] =>
            componentDisposables[index];

        internal protected virtual void RegisterDisposable(IAsyncDisposable disposable) =>
            componentDisposables.Add(disposable);

        internal protected void RegisterDisposables(IEnumerable<IAsyncDisposable> disposables)
        {
            foreach (var disposableInstance in disposables) {
                RegisterDisposable(disposableInstance);
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (componentDisposables is null) {
                return;
            }

            foreach (var moduleWrapper in componentDisposables) {
                await moduleWrapper.DisposeAsync();
            }
        }

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}
