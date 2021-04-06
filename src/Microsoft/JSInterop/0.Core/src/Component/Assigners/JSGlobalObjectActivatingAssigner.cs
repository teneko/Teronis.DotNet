// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    /// <summary>
    /// Creates a global object and sets it as result.
    /// </summary>
    public class JSGlobalObjectActivatingAssigner : IValueAssigner
    {
        private readonly IJSLocalObjectActivator jsLocalObjectActivator;

        public JSGlobalObjectActivatingAssigner(IJSLocalObjectActivator jsLocalObjectActivator) =>
            this.jsLocalObjectActivator = jsLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsLocalObjectActivator));

        public async ValueTask AssignValueAsync(IMemberDefinition definition, ValueAssignerContext context)
        {
            if (!definition.TryGetAttribute<ActivateGlobalObjectAttribute>(out var attribute)) {
                return;
            }

            var globalObjectNameOrPath = attribute.NameOrPath ?? definition.Name;
            var jsObject = await jsLocalObjectActivator.CreateInstanceAsync(globalObjectNameOrPath);
            context.SetValueResult(jsObject);
        }
    }
}
