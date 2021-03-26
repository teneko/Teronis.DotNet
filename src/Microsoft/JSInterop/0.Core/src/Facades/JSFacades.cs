// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacades<TJSFacadeActivators> : IJSFacades<TJSFacadeActivators>
        where TJSFacadeActivators : IJSFacadeActivators
    {
        public IReadOnlyList<IAsyncDisposable> JSFacadeList =>
            jsFacadesDisposables;

        public TJSFacadeActivators Activators { get; }

        private readonly List<IAsyncDisposable> jsFacadesDisposables;
        private readonly IJSCustomFacadeActivator jsCustomFacadeActivator;

        public JSFacades(
            IJSCustomFacadeActivator jsFacadeResolver,
            TJSFacadeActivators jsFacadeActivators)
        {
            jsFacadesDisposables = new List<IAsyncDisposable>();
            jsCustomFacadeActivator = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
            Activators = jsFacadeActivators ?? throw new ArgumentNullException(nameof(Activators));
            jsFacadeActivators.PrepareInstanceActivatedCapableActivators(RegisterAsyncDisposableFacade);
        }

        public IAsyncDisposable this[int index] =>
            jsFacadesDisposables[index];

        protected virtual void RegisterAsyncDisposableFacade(IAsyncDisposable jsFacadesDisposable) =>
            jsFacadesDisposables.Add(jsFacadesDisposable);

        protected internal void RegisterAsyncDisposables(IEnumerable<IAsyncDisposable> jsFacadesDisposables)
        {
            foreach (var jsFacadesDisposable in jsFacadesDisposables) {
                RegisterAsyncDisposableFacade(jsFacadesDisposable);
            }
        }

        protected T RegisterAsyncDisposable<T>(T jsFacadesDisposable)
            where T : IAsyncDisposable
        {
            RegisterAsyncDisposableFacade(jsFacadesDisposable);
            return jsFacadesDisposable;
        }

        public virtual IAsyncDisposable CreateCustomFacade(Func<TJSFacadeActivators, IJSObjectReferenceFacade> getCustomFacadeConstructorParameter, Type jsCustomFacadeType)
        {
            var customFacadeConstructorParameter = getCustomFacadeConstructorParameter(Activators);
            var jsCustomFacade = jsCustomFacadeActivator.CreateCustomFacade(customFacadeConstructorParameter, jsCustomFacadeType);
            RegisterAsyncDisposableFacade(jsCustomFacade);
            return jsCustomFacade;
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (jsFacadesDisposables is null) {
                return;
            }

            foreach (var moduleWrapper in jsFacadesDisposables) {
                await moduleWrapper.DisposeAsync();
            }
        }

        public ValueTask DisposeAsync() =>
            DisposeAsyncCore();
    }
}
