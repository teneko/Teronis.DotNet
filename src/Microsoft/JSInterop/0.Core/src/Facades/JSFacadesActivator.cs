using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivator : IInstanceActivatorBase<IJSFacades<IJSFacadeActivators>>, IJSFacadesActivator
    {
        private readonly JSFacadesActivatorOptions options;

        public JSFacadesActivator(JSFacadesActivatorOptions options) =>
            this.options = options ?? throw new ArgumentNullException(nameof(options));

        private JSFacades<TJSFacadeActivators> CreateFacades<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators
        {
            var jsFacades = options.CreateJSFacades<TJSFacadeActivators>();
            DispatchInstanceActicated((IJSFacades<IJSFacadeActivators>)jsFacades);
            return jsFacades;
        }

        public IJSFacades<TJSFacadeActivators> CreateInstance<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators =>
            CreateFacades<TJSFacadeActivators>();

        public async ValueTask<IJSFacades<TJSFacadeActivators>> CreateInstanceAsync<TJSFacadeActivators>(object component)
            where TJSFacadeActivators : IJSFacadeActivators
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacadesDisposables = new List<IAsyncDisposable>();

            foreach (var componentProperty in ComponentPropertyCollection.Create(component.GetType())) {
                foreach (var componentPropertyAssignment in options.ComponentPropertyAssigners) {
                    if ((await componentPropertyAssignment
                            .TryAssignComponentProperty(componentProperty))
                                .TryGetNotNull(out var jsFacade)) {
                        jsFacadesDisposables.Add(jsFacade);
                        componentProperty.PropertyInfo.SetValue(component, jsFacade);
                        continue;
                    }
                }
            }

            var jsFacades = CreateFacades<TJSFacadeActivators>();
            jsFacades.RegisterAsyncDisposables(jsFacadesDisposables);
            return jsFacades;
        }
    }
}
