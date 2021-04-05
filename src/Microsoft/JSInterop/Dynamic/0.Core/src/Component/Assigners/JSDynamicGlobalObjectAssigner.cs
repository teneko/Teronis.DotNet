// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Locality;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    public class JSDynamicGlobalObjectAssigner : IValueAssigner
    {
        private readonly IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator;

        public JSDynamicGlobalObjectAssigner(IJSDynamicLocalObjectActivator jsDynamicLocalObjectActivator) =>
            this.jsDynamicLocalObjectActivator = jsDynamicLocalObjectActivator ?? throw new ArgumentNullException(nameof(jsDynamicLocalObjectActivator));

        public virtual async ValueTask AssignValueAsync(IMemberDefinition definition, ValueAssignerContext context)
        {
            if (!definition.TryGetAttribute<AssignDynamicGlobalObjectAttribute>(out var attribute)) {
                return;
            }

            var globalObjectNameOrPath = attribute.NameOrPath ?? definition.Name;
            var interfaceToBeProxied = attribute.InterfaceToBeProxied ?? definition.MemberType;
            var jsObject = await jsDynamicLocalObjectActivator.CreateInstanceAsync(interfaceToBeProxied, globalObjectNameOrPath);
            context.SetValueResult(jsObject);
        }
    }
}
