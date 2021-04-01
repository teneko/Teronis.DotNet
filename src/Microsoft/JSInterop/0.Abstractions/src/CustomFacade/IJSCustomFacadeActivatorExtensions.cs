// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public static class IJSCustomFacadeActivatorExtensions
    {
        public static TCustomFacade CreateInstance<TCustomFacade>(this IJSCustomFacadeActivator jsCustomFacadeActivator,IJSObjectReferenceFacade customFacadeConstructorParameter) =>
            (TCustomFacade)jsCustomFacadeActivator.CreateInstance(customFacadeConstructorParameter, typeof(TCustomFacade));
    }
}
