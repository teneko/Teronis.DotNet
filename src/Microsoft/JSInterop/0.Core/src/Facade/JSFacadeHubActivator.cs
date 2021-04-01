// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Collections;
using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeHubActivator : IJSFacadeHubActivator
    {
        private readonly JSFacadeHubActivatorOptions options;
        private readonly JSFacadeHubActivatorPropertyAssignerOptions propertyAssignerOptions;

        public JSFacadeHubActivator(
            IOptions<JSFacadeHubActivatorOptions> options,
            IOptions<JSFacadeHubActivatorPropertyAssignerOptions> propertyAssignerOptions)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.propertyAssignerOptions = propertyAssignerOptions?.Value ?? throw new ArgumentNullException(nameof(propertyAssignerOptions));
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

            foreach (var componentMember in ComponentMemberCollection.Create(component.GetType())) {
                var context = new PropertyAssignerContext(propertyAssignerOptions.PropertyAssigners);

                await TreeIteratorExecutor<PropertyAssignerContext.PropertyAssignerEntry>.Default.ExecuteIteratorAsync(
                    context,
                    handler: entry => entry.Item.AssignPropertyAsync(componentMember, context));

                var nullableMemberResult = context.MemberResult;

                if (nullableMemberResult.IsNull) {
                    continue;
                }

                var memberResult = nullableMemberResult.Value!;
                componentMember.SetValue(component, memberResult);
                jsFacadesDisposables.Add(memberResult);
            }

            var jsFacadeHub = CreateFacades<TJSFacadeActivators>();
            jsFacadeHub.RegisterDisposables(jsFacadesDisposables);
            return jsFacadeHub;
        }
    }
}
