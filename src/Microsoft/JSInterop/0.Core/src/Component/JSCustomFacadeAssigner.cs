// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Component
{
    /// <summary>
    /// Creates a custom facade when a previous property assigner has set
    /// <see cref="PropertyAssignerContext.MemberResult"/> and when the
    /// definition is decorated with <see cref="AssignCustomFacadeAttribute"/>.
    /// </summary>
    public class JSCustomFacadeAssigner : IPropertyAssigner
    {
        private readonly IJSCustomFacadeActivator jsCustomFacadeActivator;

        public JSCustomFacadeAssigner(
            IJSCustomFacadeActivator jsCustomFacadeActivator) =>
            this.jsCustomFacadeActivator = jsCustomFacadeActivator ?? throw new ArgumentNullException(nameof(jsCustomFacadeActivator));

        /// <summary>
        /// Assigns component property with returning non-null JavaScript module facade.
        /// </summary>
        /// <returns>null/default or the JavaScript module facade.</returns>
        public virtual ValueTask AssignPropertyAsync(IDefinition componentMember, PropertyAssignerContext context)
        {
            if (context.MemberResult.TryGetNull(out var disposable)) {
                goto @return;
            }

            if (!(disposable is IJSObjectReferenceFacade jsObjectReferenceFacade)) {
                goto @return;
            }

            if (!componentMember.IsAttributeDefined<AssignCustomFacadeAttribute>()) {
                goto @return;
            }

            var customFacade = jsCustomFacadeActivator.CreateInstance(jsObjectReferenceFacade, componentMember.MemberType);
            context.MemberResult = new YetNullable<IAsyncDisposable>(customFacade);

            @return:
            return ValueTask.CompletedTask;
        }
    }
}
