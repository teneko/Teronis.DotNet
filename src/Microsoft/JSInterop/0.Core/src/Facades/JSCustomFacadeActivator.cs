// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public class JSCustomFacadeActivator : IJSCustomFacadeActivator
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IJSCustomFacadeDictionary jsFacadeDictionary;

        public JSCustomFacadeActivator(
            IServiceProvider serviceProvider,
            JSCustomFacadeActivatorOptions options)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            jsFacadeDictionary = options.JSFacadeDictionaryBuilder.Build();
        }

        public virtual IAsyncDisposable CreateCustomFacade(IJSObjectReferenceFacade customFacadeConstructorParameter, Type jsCustomFacadeType)
        {
            if (customFacadeConstructorParameter is null) {
                throw new ArgumentNullException(nameof(customFacadeConstructorParameter));
            }

            if (!jsFacadeDictionary.TryGetValue(jsCustomFacadeType, out var jsFacadeCreatorHandler)) {
                throw new NotSupportedException($"Type {jsCustomFacadeType} is not supported.");
            }

            if (!(jsFacadeCreatorHandler is null)) {
                return jsFacadeCreatorHandler.Invoke(customFacadeConstructorParameter);
            }

            return (IAsyncDisposable)ActivatorUtilities.CreateInstance(serviceProvider, jsCustomFacadeType, customFacadeConstructorParameter);
        }
    }
}
