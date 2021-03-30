// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadesActivator : InstanceActivatorBase<IJSFacades<IJSFacadeActivators>>, IJSFacadesActivator
    {
        private readonly JSFacadesActivatorOptions options;

        public JSFacadesActivator(IOptions<JSFacadesActivatorOptions> options) =>
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));

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
                foreach (var componentPropertyAssignment in options.PropertyAssigners) {
                    if ((await componentPropertyAssignment
                            .TryAssignProperty(componentProperty))
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
