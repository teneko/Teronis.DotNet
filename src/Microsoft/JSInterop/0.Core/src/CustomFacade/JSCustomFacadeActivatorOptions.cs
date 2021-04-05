// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeActivatorOptions
    {
        internal JSCustomFacadeServiceCollection CustomFacadeServices { get; }

        public JSCustomFacadeActivatorOptions() =>
            CustomFacadeServices = new JSCustomFacadeServiceCollection();

        public void ConfigureCustomFacadeServices(Action<IJSCustomFacadeServiceCollection> callback) =>
            callback?.Invoke(CustomFacadeServices);
    }
}
