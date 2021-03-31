// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Component;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Interceptors
{
    public class JSDynamicModuleActivatingInterceptor : JSObjectInterceptor
    {
        private readonly IJSDynamicModulePropertyAssigner propertyAssigner;

        public JSDynamicModuleActivatingInterceptor(IJSDynamicModulePropertyAssigner propertyAssigner) =>
            this.propertyAssigner = propertyAssigner ?? throw new System.ArgumentNullException(nameof(propertyAssigner));

        public override async ValueTask InterceptInvokeAsync<TTaskArgument>(IJSObjectInvocation<TTaskArgument> invocation)
        {
            if (!(await propertyAssigner.TryAssignProperty(invocation.Definition)).TryGetNotNull(out var jsModule)) {
                return;
            }

            invocation.SetDeterminedResult(new ValueTask<TTaskArgument>((TTaskArgument)jsModule));
        }
    }
}
