// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public class JSObjectInterceptorBuilderOptionsConfiguration<DerivedType> : IConfigureOptions<JSIteratingObjectInterceptorBuilderOptions<DerivedType>>
        where DerivedType : JSIteratingObjectInterceptorBuilderOptions<DerivedType>
    {
        public static JSObjectInterceptorBuilderOptionsConfiguration<DerivedType> Create(IServiceProvider serviceProvider) =>
            ActivatorUtilities.CreateInstance<JSObjectInterceptorBuilderOptionsConfiguration<DerivedType>>(serviceProvider);

        private readonly IServiceProvider serviceProvider;

        public JSObjectInterceptorBuilderOptionsConfiguration(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Configure(JSIteratingObjectInterceptorBuilderOptions<DerivedType> options)
        {
            options.ServiceProvider = serviceProvider;
        }
    }
}
