using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Module;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacades : IJSFacades
    {
        public IReadOnlyList<IAsyncDisposable> JSFacadeList =>
            jsFacadesDisposables;

        private readonly List<IAsyncDisposable> jsFacadesDisposables;
        private readonly IJSFacadeResolver jsFacadeResolver;
        private readonly IJSModuleActivator jsModuleActivator;
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSFacades(
            IJSFacadeResolver jsFacadeResolver,
            IJSModuleActivator jsModuleActivator,
            IJSLocalObjectActivator jsLocalObjectActivator)
        {
            jsFacadesDisposables = new List<IAsyncDisposable>();
            this.jsFacadeResolver = jsFacadeResolver ?? throw new ArgumentNullException(nameof(jsFacadeResolver));
            this.jsModuleActivator = jsModuleActivator ?? throw new ArgumentNullException(nameof(jsModuleActivator));
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));
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


        public async ValueTask<IJSModule> CreateModuleAsync(string moduleNameOrPath) =>
            RegisterAsyncDisposable(await jsModuleActivator.CreateInstanceAsync(moduleNameOrPath));


        public async ValueTask<IAsyncDisposable> CreateModuleFacadeAsync(string moduleNameOrPath, Type jsFacadeType) =>
            RegisterAsyncDisposable(await jsFacadeResolver.CreateModuleFacadeAsync(moduleNameOrPath, jsFacadeType));


        public IJSLocalObject CreateInstance(IJSObjectReference jsObjectReference) =>
            RegisterAsyncDisposable(jsLocalObjectActivator.CreateInstance(jsObjectReference));

        public async ValueTask<IJSLocalObject> CreateInstanceAsync(string objectName) =>
            RegisterAsyncDisposable(await jsLocalObjectActivator.CreateInstanceAsync(objectName));

        public async ValueTask<IJSLocalObject> CreateInstanceAsync(IJSObjectReference jsObjectReference, string objectName) =>
            RegisterAsyncDisposable(await jsLocalObjectActivator.CreateInstanceAsync(jsObjectReference, objectName));


        public IAsyncDisposable CreateLocalObjectFacade(IJSObjectReference jsObjectReference, Type jsFacadeType) =>
            RegisterAsyncDisposable(jsFacadeResolver.CreateLocalObjectFacade(jsObjectReference, jsFacadeType));

        public async ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(string objectName, Type jsFacadeType) =>
            RegisterAsyncDisposable(await jsFacadeResolver.CreateLocalObjectFacadeAsync(objectName, jsFacadeType));

        public async ValueTask<IAsyncDisposable> CreateLocalObjectFacadeAsync(IJSObjectReference jsObjectReference, string objectName, Type jsFacadeType) =>
            RegisterAsyncDisposable(await jsFacadeResolver.CreateLocalObjectFacadeAsync(jsObjectReference, objectName, jsFacadeType));


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
