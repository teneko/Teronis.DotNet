// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigner
{
    public static class IValueAssignerExtensions
    {
        public static ValueTask AssignPropertyAsync(this IValueAssigner propertyAssigner, IDefinition definition) =>
            propertyAssigner.AssignValueAsync(definition, new ValueAssignerContext(propertyAssigner));
    }
}
