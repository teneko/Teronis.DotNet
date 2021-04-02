// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeHubActivator : IJSFacadeHubActivator
    {
        private readonly JSFacadeHubActivatorOptions options;
        private readonly ValueAssignerList<JSFacadeHubActivatorValueAssignerOptions> propertyAssignerList;

        public JSFacadeHubActivator(
            IOptions<JSFacadeHubActivatorOptions> options,
            ValueAssignerList<JSFacadeHubActivatorValueAssignerOptions> propertyAssignerList)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.propertyAssignerList = propertyAssignerList;
        }

        private JSFacadeHub<TJSFacadeActivators> CreateFacades<TJSFacadeActivators>()
            where TJSFacadeActivators : class
        {
            var jsFacadeHub = options.CreateFacadeHub<TJSFacadeActivators>();
            return jsFacadeHub;
        }

        public IJSFacadeHub<TJSFacadeActivators> CreateInstance<TJSFacadeActivators>()
            where TJSFacadeActivators : class =>
            CreateFacades<TJSFacadeActivators>();

        public async ValueTask<IJSFacadeHub<TJSFacadeActivators>> CreateInstanceAsync<TJSFacadeActivators>(object component)
            where TJSFacadeActivators : class
        {
            if (component is null) {
                throw new ArgumentNullException(nameof(component));
            }

            var jsFacadesDisposables = new List<IAsyncDisposable>();

            foreach (var componentMember in ComponentMemberCollection.Create(component, component.GetType())) {
                var context = new ValueAssignerContext(propertyAssignerList);

                if (await ValueAssignerIteratorExecutor.TryAssignValueAsync(componentMember, context)) {
                    var memberResult = context.ValueResult.Value!;
                    componentMember.SetValue(memberResult);
                    jsFacadesDisposables.Add(memberResult);
                }
            }

            var jsFacadeHub = CreateFacades<TJSFacadeActivators>();
            jsFacadeHub.RegisterDisposables(jsFacadesDisposables);
            return jsFacadeHub;
        }
    }
}
