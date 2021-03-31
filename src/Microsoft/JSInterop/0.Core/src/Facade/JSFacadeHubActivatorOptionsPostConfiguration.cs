// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Options;
using Teronis.Microsoft.JSInterop.Facade;

namespace Teronis.Microsoft.JSInterop
{
    internal class JSFacadeHubActivatorOptionsPostConfiguration : IConfigureOptions<JSFacadeHubActivatorOptions>
    {
        private readonly IServiceProvider serviceProvider;

        public JSFacadeHubActivatorOptionsPostConfiguration(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public void Configure(JSFacadeHubActivatorOptions options) =>
            options.SetServiceProvider(serviceProvider);
    }
}
