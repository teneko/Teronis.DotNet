// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public sealed class JSInterceptorBuilderOptionsPostConfiguration<TDerivedBuilderOptions, TDerivedAssignerOptions> : IPostConfigureOptions<TDerivedBuilderOptions>
        where TDerivedBuilderOptions : JSInterceptorBuilderOptions<TDerivedBuilderOptions>
        where TDerivedAssignerOptions : ValueAssignerOptions<TDerivedAssignerOptions>
    {
        private readonly JSGlobalInterceptorBuilderOptions globalOptions;
        private readonly ValueAssignerList<TDerivedAssignerOptions> propertyAssignerList;
        private readonly IEnumerable<PostConfigureInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks;

        public JSInterceptorBuilderOptionsPostConfiguration(
            IOptions<JSGlobalInterceptorBuilderOptions> globalOptions,
            ValueAssignerList<TDerivedAssignerOptions> propertyAssignerList,
            IEnumerable<PostConfigureInterceptorBuilderDelegate<TDerivedBuilderOptions, TDerivedAssignerOptions>> postConfigureInterceptorBuilderCallbacks)
        {
            this.globalOptions = globalOptions?.Value ?? throw new ArgumentNullException(nameof(globalOptions));
            this.propertyAssignerList = propertyAssignerList;
            this.postConfigureInterceptorBuilderCallbacks = postConfigureInterceptorBuilderCallbacks;
        }

        public void PostConfigure(string _, TDerivedBuilderOptions options)
        {
            var propertyAssignerOptions = propertyAssignerList.Options;

            foreach (var postConfigureBuilderCallback in postConfigureInterceptorBuilderCallbacks) {
                postConfigureBuilderCallback(options, propertyAssignerOptions);
            }

            options.SetValueAssignerOptions(propertyAssignerOptions);
            options.SetValueAssignerList(propertyAssignerList);

            if (!options.CreateInterceptorServicesWhenUserUntouched()) {
                return;
            }

            if (globalOptions.AreInterceptorServicesUserTouched) {
                options.InterceptorServices.UseExtension(extension =>
                    extension.Add(globalOptions.InterceptorServices));
            } else {
                options.ConfigureInterceptorServices(builder => builder
                    .AddDefaultNonDynamicInterceptors()
                    .AddIterativeValueAssignerInterceptor());
            }
        }
    }
}
