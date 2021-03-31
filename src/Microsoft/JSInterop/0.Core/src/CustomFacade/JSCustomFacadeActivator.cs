// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeActivator : IJSCustomFacadeActivator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IJSCustomFacadeDictionary jsFacadeDictionary;

        public JSCustomFacadeActivator(
            IServiceProvider serviceProvider,
            IOptions<JSCustomFacadeActivatorOptions> options)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            jsFacadeDictionary = options.Value.JSFacadeDictionaryBuilder.Build();
        }

        public virtual IAsyncDisposable CreateInstance(IJSObjectReferenceFacade customFacadeConstructorParameter, Type jsCustomFacadeType)
        {
            if (customFacadeConstructorParameter is null) {
                throw new ArgumentNullException(nameof(customFacadeConstructorParameter));
            }

            if (!jsFacadeDictionary.TryGetValue(jsCustomFacadeType, out var jsFacadeCreatorHandler)) {
                throw new NotSupportedException($"Type {jsCustomFacadeType} is not supported.");
            }

            var jsCustomFacadeFactoryServiceProvider = new JSCustomFacadeFactoryServiceProvider(serviceProvider, customFacadeConstructorParameter);

            if (!(jsFacadeCreatorHandler is null)) {
                return jsFacadeCreatorHandler.Invoke(jsCustomFacadeFactoryServiceProvider);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(jsCustomFacadeFactoryServiceProvider, jsCustomFacadeType);
        }
    }
}
