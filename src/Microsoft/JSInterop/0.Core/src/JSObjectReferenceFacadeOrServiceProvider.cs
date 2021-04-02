// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.CustomFacade;
using Teronis.Microsoft.JSInterop.Utils;

namespace Teronis.Microsoft.JSInterop
{
    /// <summary>
    /// This class tries to return the JavaScript reference when it implements the requested serivce type.
    /// </summary>
    internal sealed class JSObjectReferenceFacadeOrServiceProvider : IJSCustomFacadeFactoryServiceProvider
    {
        public IJSObjectReferenceFacade JSObjectReferenceFacade { get; }

        private readonly IServiceProvider serviceProvider;
        private readonly HashSet<Type> jsObjectReferenceFacadeInterfaceTypes;

        public JSObjectReferenceFacadeOrServiceProvider(IServiceProvider serviceProvider, IJSObjectReferenceFacade jsObjectReferenceFacade)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            JSObjectReferenceFacade = jsObjectReferenceFacade ?? throw new ArgumentNullException(nameof(jsObjectReferenceFacade));
            jsObjectReferenceFacadeInterfaceTypes = TypeUtils.GetInterfaces(jsObjectReferenceFacade.GetType());
        }

        public object? GetService(Type serviceType)
        {
            if (jsObjectReferenceFacadeInterfaceTypes.Contains(serviceType)) {
                return JSObjectReferenceFacade;
            }

            return serviceProvider.GetService(serviceType);
        }
    }
}
