// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Interception.Interceptors;

namespace Teronis.Microsoft.JSInterop.Interception.ServiceBuilder
{
    public class JSInterceptorBuilder<TInterceptorBuilderOptions> : IJSInterceptorBuilder
        where TInterceptorBuilderOptions : JSInterceptorBuilderOptions<TInterceptorBuilderOptions>
    {
        public TInterceptorBuilderOptions Options { get; }

        private readonly IServiceProvider serviceProvider;
        private IJSInterceptor? interceptor;

        public JSInterceptorBuilder(IOptions<TInterceptorBuilderOptions> options, IServiceProvider serviceProvider)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.serviceProvider = new BuildingInterceptorSeviceProvider(serviceProvider, Options.ValueAssigners);
        }

        public IJSInterceptor BuildInterceptor()
        {
            if (!(interceptor is null)) {
                return interceptor;
            }

            var interceptorServices = Options.InterceptorServices;
            return interceptor = interceptorServices.Build(serviceProvider);
        }
    }
}
