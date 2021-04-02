// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Interception.Interceptor.Builder
{
    public class JSMutableInterceptorBuilder<TInterceptorBuilderOptions> : IJSMutableInterceptorBuilder
        where TInterceptorBuilderOptions : JSInterceptorBuilderOptions<TInterceptorBuilderOptions>
    {
        public TInterceptorBuilderOptions Options { get; }

        private readonly IServiceProvider serviceProvider;
        private IJSInterceptor? interceptor;

        public JSMutableInterceptorBuilder(IOptions<TInterceptorBuilderOptions> options, IServiceProvider serviceProvider)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.serviceProvider = new BuildingInterceptorSeviceProvider(serviceProvider, Options.ValueAssigners);
        }

        public IJSInterceptor BuildInterceptor(
            Action<IJSInterceptorBuilder>? configureBuilder)
        {
            if (configureBuilder is null && !(interceptor is null)) {
                return interceptor;
            }

            var interceptorBuilder = Options.InterceptorBuilder;

            if (configureBuilder is null) {
                return interceptor = interceptorBuilder.Build(serviceProvider);
            }

            var mutatingInterceptorBuilder = new JSInterceptorBuilder(interceptorBuilder.InterceptorDescriptors);
            mutatingInterceptorBuilder.SetRegistrationPhase(InterceptorDescriptorRegistrationPhase.FacadeActivation);
            configureBuilder(mutatingInterceptorBuilder);
            return mutatingInterceptorBuilder.Build(serviceProvider);
        }
    }
}
