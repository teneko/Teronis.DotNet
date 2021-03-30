// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSFacadeHubActivator : InstanceActivatorBase<IJSFacadeHub<IJSFacadeActivators>>, IJSFacadeHubActivator
    {
        private readonly JSFacadeHubActivatorOptions options;

        public JSFacadeHubActivator(IOptions<JSFacadeHubActivatorOptions> options) =>
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));

        private JSFacadeHub<TJSFacadeActivators> CreateFacades<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators
        {
            var jsFacadeHub = options.CreateFacadeHub<TJSFacadeActivators>();
            DispatchInstanceActicated((IJSFacadeHub<IJSFacadeActivators>)jsFacadeHub);
            return jsFacadeHub;
        }

        public IJSFacadeHub<TJSFacadeActivators> CreateInstance<TJSFacadeActivators>()
            where TJSFacadeActivators : IJSFacadeActivators =>
            CreateFacades<TJSFacadeActivators>();

        public async ValueTask<IJSFacadeHub<TJSFacadeActivators>> CreateInstanceAsync<TJSFacadeActivators>(object component)
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

            var jsFacadeHub = CreateFacades<TJSFacadeActivators>();
            jsFacadeHub.RegisterAsyncDisposables(jsFacadesDisposables);
            return jsFacadeHub;
        }
    }
}
