// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Collections;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component.ValueAssigners
{
    internal static class ValueAssignerIteratorExecutor
    {
        public static async Task<bool> TryAssignValueAsync<TDefinition>(TDefinition definition, ValueAssignerContext context)
            where TDefinition : IMemberDefinition
        {
            await TreeIteratorExecutor<ValueAssignerContext.ValueAssignerEntry>.Default.ExecuteIteratorAsync(
                context,
                handler: entry => entry.Item.AssignValueAsync(definition, context))
                .ConfigureAwait(false);

            if (context.ValueResult.IsNull) {
                return false;
            }

            return true;
        }
    }
}
