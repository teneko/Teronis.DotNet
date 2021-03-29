// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Teronis.Microsoft.JSInterop.Internals.Utils;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public sealed class JSCustomFacadeFactoryServiceProvider : IJSCustomFacadeFactoryServiceProvider
    {
        public IJSObjectReferenceFacade JSObjectReferenceFacade { get; }

        private readonly IServiceProvider serviceProvider;
        private readonly HashSet<Type> jsObjectReferenceFacadeInterfaceTypes;

        public JSCustomFacadeFactoryServiceProvider(IServiceProvider serviceProvider, IJSObjectReferenceFacade jsObjectReferenceFacade)
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
