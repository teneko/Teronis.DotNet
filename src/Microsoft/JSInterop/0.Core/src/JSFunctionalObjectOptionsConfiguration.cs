// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop
{
    public class JSFunctionalObjectOptionsConfiguration<DerivedType> : IConfigureOptions<JSFunctionalObjectOptions<DerivedType>>
        where DerivedType : JSFunctionalObjectOptions<DerivedType>
    {
        public static JSFunctionalObjectOptionsConfiguration<DerivedType> Create(IServiceProvider serviceProvider) =>
            ActivatorUtilities.CreateInstance<JSFunctionalObjectOptionsConfiguration<DerivedType>>(serviceProvider);

        private readonly IServiceProvider serviceProvider;

        public JSFunctionalObjectOptionsConfiguration(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Configure(JSFunctionalObjectOptions<DerivedType> options) {
            options.ServiceProvider = serviceProvider;
        }
    }
}
