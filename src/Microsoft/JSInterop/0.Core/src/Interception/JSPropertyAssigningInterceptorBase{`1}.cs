// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component.Assigners;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public abstract class JSPropertyAssigningInterceptorBase<TValueAssigner> : JSInterceptor
        where TValueAssigner : IValueAssigner
    {
        private readonly TValueAssigner propertyAssigner;

        public JSPropertyAssigningInterceptorBase(TValueAssigner propertyAssigner) =>
            this.propertyAssigner = propertyAssigner ?? throw new System.ArgumentNullException(nameof(propertyAssigner));

        public override async ValueTask InterceptInvokeAsync<TTaskArgument>(IJSObjectInvocation<TTaskArgument> invocation, InterceptionContext context)
        {
            var propertyAssignerContext = new ValueAssignerContext(propertyAssigner);
            await propertyAssigner.AssignValueAsync(invocation.InvocationDefinition, propertyAssignerContext);

            if (propertyAssignerContext.ValueResult.TryGetNull(out var instance)) {
                return;
            }

            invocation.SetAlternativeResult(new ValueTask<TTaskArgument>((TTaskArgument)instance));
        }
    }
}
