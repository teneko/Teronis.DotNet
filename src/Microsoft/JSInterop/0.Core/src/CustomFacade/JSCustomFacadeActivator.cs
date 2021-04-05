// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeActivator : IJSCustomFacadeActivator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILookup<Type, JSCustomFacadeServiceDescriptor> jsCustomFacadeServiceLookup;

        public JSCustomFacadeActivator(
            IServiceProvider serviceProvider,
            IOptions<JSCustomFacadeActivatorOptions> options)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            jsCustomFacadeServiceLookup = options.Value.CustomFacadeServices.ToLookup(x => x.ServiceType);
        }

        public virtual IAsyncDisposable CreateInstance(IJSObjectReferenceFacade customFacadeConstructorParameter, Type jsCustomFacadeType)
        {
            if (customFacadeConstructorParameter is null) {
                throw new ArgumentNullException(nameof(customFacadeConstructorParameter));
            }

            if (!jsCustomFacadeServiceLookup.Contains(jsCustomFacadeType)) {
                throw new NotSupportedException($"Type {jsCustomFacadeType} is not supported.");
            }

            var customFacadeServiceDescriptor = jsCustomFacadeServiceLookup[jsCustomFacadeType].Last();

            var jsCustomFacadeFactoryServiceProvider = new JSObjectReferenceFacadeOrServiceProvider(serviceProvider, customFacadeConstructorParameter);

            if (!(customFacadeServiceDescriptor.ImplementationFactory is null)) {
                return (IAsyncDisposable)customFacadeServiceDescriptor.ImplementationFactory.Invoke(jsCustomFacadeFactoryServiceProvider);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(jsCustomFacadeFactoryServiceProvider, customFacadeServiceDescriptor.ImplementationType!);
        }
    }
}
