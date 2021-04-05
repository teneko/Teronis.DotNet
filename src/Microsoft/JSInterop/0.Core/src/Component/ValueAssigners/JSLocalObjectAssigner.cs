// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners
{
    /// <summary>
    /// Assigns global object.
    /// </summary>
    public class JSLocalObjectAssigner : IValueAssigner
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSLocalObjectAssigner(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public async ValueTask AssignValueAsync(IMemberDefinition definition, ValueAssignerContext context)
        {
            if (!definition.TryGetAttribute<AssignGlobalObject>(out var attribute)) {
                return;
            }

            var globalObjectName = attribute.GlobalObjectName ?? definition.Name;
            var jsObject = await jsLocalObjectActivator.CreateInstanceAsync(globalObjectName);
            context.ValueResult = new YetNullable<IAsyncDisposable>(jsObject);
        }
    }
}
