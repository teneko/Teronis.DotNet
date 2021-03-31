// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public class JSFacadeHubActivatorOptions
    {
        private IServiceProvider? serviceProvider;

        internal void SetServiceProvider(IServiceProvider serviceProvider) =>
            this.serviceProvider = serviceProvider;

        private IServiceProvider GetServiceProviderOrThrow()
        {
            if (serviceProvider is null) {
                throw new InvalidOperationException(nameof(serviceProvider));
            }

            return serviceProvider;
        }

        internal TJSFacadeActivators CreateFacadeActivators<TJSFacadeActivators>(IServiceProvider? serviceProvider)
        {
            serviceProvider ??= serviceProvider ?? GetServiceProviderOrThrow();
            return ActivatorUtilities.GetServiceOrCreateInstance<TJSFacadeActivators>(serviceProvider);
        }

        internal JSFacadeHub<TJSFacadeActivators> CreateFacadeHub<TJSFacadeActivators>()
            where TJSFacadeActivators : class
        {
            var serviceProvider = GetServiceProviderOrThrow();
            var jsFacadeActivators = CreateFacadeActivators<TJSFacadeActivators>(serviceProvider);
            return ActivatorUtilities.CreateInstance<JSFacadeHub<TJSFacadeActivators>>(serviceProvider, jsFacadeActivators);
        }
    }
}
