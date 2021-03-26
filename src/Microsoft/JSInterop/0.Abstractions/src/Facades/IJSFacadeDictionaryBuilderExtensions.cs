// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Microsoft.JSInterop.Facades
{
    public static class IJSFacadeDictionaryBuilderExtensions
    {
        public static IJSCustomFacadeDictionaryBuilder AddDefault(this IJSCustomFacadeDictionaryBuilder builder) =>
            builder.Add(module => module);
    }
}
