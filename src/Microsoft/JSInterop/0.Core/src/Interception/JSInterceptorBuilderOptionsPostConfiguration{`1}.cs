// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public sealed class JSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions, TDerivedAssignerOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
        where TDerivedAssignerOptions : PropertyAssignerOptions<TDerivedAssignerOptions>
    {
        private readonly TDerivedAssignerOptions propertyAssignerOptions;
        private readonly IServiceProvider serviceProvider;
        private readonly IEnumerable<LateConfigureInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks;

        public JSInterceptorBuilderOptionsPostConfiguration(
            IServiceProvider serviceProvider,
            IOptions<TDerivedAssignerOptions> propertyAssignerOptions, 
            IEnumerable<LateConfigureInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.propertyAssignerOptions = propertyAssignerOptions.Value;
            this.postConfigureInterceptorBuilderCallbacks = postConfigureInterceptorBuilderCallbacks;
        }

        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            options.SetServiceProvider(serviceProvider);

            foreach (var postConfigureBuilderCallback in postConfigureInterceptorBuilderCallbacks) {
                postConfigureBuilderCallback(options, propertyAssignerOptions);
            }

            // TODO: Add default interceptors (property assigner -> interceptor conversion)
        }
    }
}
