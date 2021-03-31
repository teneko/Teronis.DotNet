// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Interceptors
{
    public abstract class InstanceActivatingInterceptor<TPropertyAssigner> : JSObjectInterceptor
        where TPropertyAssigner : IPropertyAssigner
    {
        private readonly TPropertyAssigner propertyAssigner;

        public InstanceActivatingInterceptor(TPropertyAssigner propertyAssigner) =>
            this.propertyAssigner = propertyAssigner ?? throw new System.ArgumentNullException(nameof(propertyAssigner));

        public override async ValueTask InterceptInvokeAsync<TTaskArgument>(IJSObjectInvocation<TTaskArgument> invocation)
        {
            if (!(await propertyAssigner.TryAssignProperty(invocation.Definition)).TryGetNotNull(out var instance)) {
                return;
            }

            invocation.SetDeterminedResult(new ValueTask<TTaskArgument>((TTaskArgument)instance));
        }
    }
}
