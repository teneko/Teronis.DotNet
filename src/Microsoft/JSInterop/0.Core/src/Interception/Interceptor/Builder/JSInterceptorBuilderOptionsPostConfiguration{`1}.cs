// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ValueAssigner.Builder;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public sealed class JSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions, TDerivedAssignerOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
        where TDerivedAssignerOptions : ValueAssignerOptions<TDerivedAssignerOptions>
    {
        private readonly ValueAssignerList<TDerivedAssignerOptions> propertyAssignerList;
        private readonly IEnumerable<ConfigureMutableInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks;

        public JSInterceptorBuilderOptionsPostConfiguration(
            ValueAssignerList<TDerivedAssignerOptions> propertyAssignerList,
            IEnumerable<ConfigureMutableInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks)
        {
            this.propertyAssignerList = propertyAssignerList;
            this.postConfigureInterceptorBuilderCallbacks = postConfigureInterceptorBuilderCallbacks;
        }

        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            foreach (var postConfigureBuilderCallback in postConfigureInterceptorBuilderCallbacks) {
                postConfigureBuilderCallback(options, propertyAssignerList.Options);
            }

            options.SetValueAssignerOptions(propertyAssignerList.Options);
            options.SetValueAssignerList(propertyAssignerList);

            if (!options.TryCreateInterceptorBuilderUserUntouched()) {
                return;
            }

            options.ConfigureInterceptorBuilder(builder => builder
                .AddDefaultNonDynamicInterceptors()
                .AddIterativeValueAssignerInterceptor());
        }
    }
}
