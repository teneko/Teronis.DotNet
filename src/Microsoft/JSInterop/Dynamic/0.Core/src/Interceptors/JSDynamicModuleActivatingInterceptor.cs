// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;
using Teronis.Microsoft.JSInterop.Dynamic.Activators;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;
using Teronis.Microsoft.JSInterop.Interception;

namespace Teronis.Microsoft.JSInterop.Dynamic.Interceptors
{
    public class JSDynamicModuleActivatingInterceptor : JSObjectInterceptor
    {
        private readonly IJSDynamicModuleActivator jsDynamicModuleActivator;

        public JSDynamicModuleActivatingInterceptor(IJSDynamicModuleActivator jsDynamicModuleActivator) =>
            this.jsDynamicModuleActivator = jsDynamicModuleActivator ?? throw new System.ArgumentNullException(nameof(jsDynamicModuleActivator));

        public override async ValueTask InterceptInvokeAsync<TValue>(IJSObjectInvocation<TValue> invocation)
        {
            if (!invocation.MemberAttributes.TryGetAttribute<JSDynamicModuleActivatingInterceptorAttribute>(out var propertyAttribute)) {
                return;
            }

            var moduleNameOrPath = JSModuleAttributeUtils.GetModuleNameOrPath<JSModuleClassAttribute>(propertyAttribute, invocation.TaskArgumentTypeAttributes);
            var module = (TValue)await jsDynamicModuleActivator.CreateInstanceAsync(typeof(TValue), moduleNameOrPath);
            invocation.SetDeterminedResult(new ValueTask<TValue>(module));
        }
    }
}
