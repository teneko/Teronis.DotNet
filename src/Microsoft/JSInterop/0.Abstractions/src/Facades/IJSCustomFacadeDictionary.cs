// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{   
    public interface IJSCustomFacadeDictionary : IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>
    { }
}
