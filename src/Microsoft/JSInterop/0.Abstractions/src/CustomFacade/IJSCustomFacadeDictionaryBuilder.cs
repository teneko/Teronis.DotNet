// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.CustomFacade
{
    public interface IJSCustomFacadeDictionaryBuilder : IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>
    {
        IJSCustomFacadeDictionaryBuilder Add(Type jsFacadeType, JSCustomFacadeFactoryDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate = null);

        IJSCustomFacadeDictionaryBuilder Add<TCustomFacade>(JSCustomFacadeFactoryDelegate<TCustomFacade>? jsFacadeCreatorHandler = null)
            where TCustomFacade : class, IAsyncDisposable;

        IJSCustomFacadeDictionaryBuilder Remove(Type jsFacadeType);

        IJSCustomFacadeDictionaryBuilder Remove<T>()
            where T : IAsyncDisposable;

        public IJSCustomFacadeDictionaryBuilder Clear();
    }
}
