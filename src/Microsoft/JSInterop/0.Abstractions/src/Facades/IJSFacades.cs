// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacades<out TJSFacadeActivators> : IAsyncDisposable
        where TJSFacadeActivators : IJSFacadeActivators
    {
        TJSFacadeActivators Activators { get; }

        IAsyncDisposable CreateCustomFacade(Func<TJSFacadeActivators, IJSObjectReferenceFacade> getCustomFacadeConstructorParameter, Type jsCustomFacadeType);
    }
}
