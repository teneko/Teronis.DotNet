// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.Assigners
{
    public interface IValueAssigner
    {
        /// <summary>
        /// Assigns the component property with returning non-null JavaScript facade.
        /// </summary>
        /// <param name="definition">The component property.</param>
        /// <param name="context"></param>
        /// <returns>"Null"/default or the JavaScript facade.</returns>
        ValueTask AssignValueAsync(IMemberDefinition definition, ValueAssignerContext context);
    }
}
