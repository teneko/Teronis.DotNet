// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public class JSCustomFacadeActivatorOptions
    {
        public IJSCustomFacadeDictionaryBuilder JSFacadeDictionaryConfiguration =>
            JSFacadeDictionaryBuilder;

        internal readonly JSCustomFacadeDictionaryBuilder JSFacadeDictionaryBuilder;

        public JSCustomFacadeActivatorOptions() =>
            JSFacadeDictionaryBuilder = new JSCustomFacadeDictionaryBuilder()
                .AddDefault();
    }
}
