// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    public static class IPropertyAssignerExtensions
    {
        public static ValueTask AssignPropertyAsync(this IPropertyAssigner propertyAssigner, IDefinition definition) =>
            propertyAssigner.AssignPropertyAsync(definition, new PropertyAssignerContext(propertyAssigner));
    }
}
