// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Component.ServiceBuilder;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public sealed class JSInterceptorServicesOptionsPostConfiguration<TInterceptorServicesOptions, TAssignerServicesOptions> : IPostConfigureOptions<TInterceptorServicesOptions>
        where TInterceptorServicesOptions : JSInterceptorServicesOptions<TInterceptorServicesOptions>
        where TAssignerServicesOptions : ValueAssignerServicesOptions<TAssignerServicesOptions>
    {
        private readonly JSGlobalInterceptorServicesOptions globalOptions;
        private readonly ValueAssignerList<TAssignerServicesOptions> propertyAssignerList;
        private readonly IEnumerable<PostConfigureInterceptorServicesOptionsDelegate<TInterceptorServicesOptions, TAssignerServicesOptions>> postConfigureInterceptorBuilderCallbacks;

        public JSInterceptorServicesOptionsPostConfiguration(
            IOptions<JSGlobalInterceptorServicesOptions> globalOptions,
            ValueAssignerList<TAssignerServicesOptions> propertyAssignerList,
            IEnumerable<PostConfigureInterceptorServicesOptionsDelegate<TInterceptorServicesOptions, TAssignerServicesOptions>> postConfigureInterceptorBuilderCallbacks)
        {
            this.globalOptions = globalOptions?.Value ?? throw new ArgumentNullException(nameof(globalOptions));
            this.propertyAssignerList = propertyAssignerList;
            this.postConfigureInterceptorBuilderCallbacks = postConfigureInterceptorBuilderCallbacks;
        }

        public void PostConfigure(string _, TInterceptorServicesOptions options)
        {
            var propertyAssignerServicesOptions = propertyAssignerList.Options;

            foreach (var postConfigureBuilderCallback in postConfigureInterceptorBuilderCallbacks) {
                postConfigureBuilderCallback(options, propertyAssignerServicesOptions);
            }

            options.SetValueAssignerServicesOptions(propertyAssignerServicesOptions);
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
