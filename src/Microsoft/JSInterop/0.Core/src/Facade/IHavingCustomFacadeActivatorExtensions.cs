// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public static class IHavingCustomFacadeActivatorExtensions
    {
        public static IAsyncDisposable CreateCustomFacade<TFacadeActivators>(this TFacadeActivators facadeActivators, Func<TFacadeActivators, IJSObjectReferenceFacade> getCustomFacadeConstructorParameter, Type jsCustomFacadeType)
            where TFacadeActivators : IHavingCustomFacadeActivator
        {
            var customFacadeConstructorParameter = getCustomFacadeConstructorParameter(facadeActivators);
            var jsCustomFacade = facadeActivators.JSCustomFacadeActivator.CreateInstance(customFacadeConstructorParameter, jsCustomFacadeType);
            return jsCustomFacade;
        }
    }
}
