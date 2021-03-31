// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Microsoft.JSInterop.CustomFacade;

namespace Teronis.Microsoft.JSInterop.Facade
{
    public interface IHavingCustomFacadeActivator
    { 
        IJSCustomFacadeActivator JSCustomFacadeActivator { get; }
    }
}
