using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSCustomFacadeDictionaryBuilder : IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>
    {
        IJSCustomFacadeDictionaryBuilder Add(Type jsFacadeType, JSCustomFacadeFactoryDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate = null);

        IJSCustomFacadeDictionaryBuilder Add<T>(JSCustomFacadeFactoryDelegate<T>? jsFacadeCreatorHandler = null)
            where T : class, IAsyncDisposable;

        IJSCustomFacadeDictionaryBuilder Remove(Type jsFacadeType);

        IJSCustomFacadeDictionaryBuilder Remove<T>()
            where T : IAsyncDisposable;

        public IJSCustomFacadeDictionaryBuilder Clear();
    }
}
