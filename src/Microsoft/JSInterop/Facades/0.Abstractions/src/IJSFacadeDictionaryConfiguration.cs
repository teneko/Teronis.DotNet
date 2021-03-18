using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSFacadeDictionaryConfiguration : IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>
    {
        IJSFacadeDictionaryConfiguration Add(Type jsFacadeType, JSFacadeCreatorDelegate<IAsyncDisposable>? jsFacadeCreatorDelegate = null);

        IJSFacadeDictionaryConfiguration Add<T>(JSFacadeCreatorDelegate<T>? jsFacadeCreatorHandler = null)
            where T : class, IAsyncDisposable;

        IJSFacadeDictionaryConfiguration Remove(Type jsFacadeType);

        IJSFacadeDictionaryConfiguration Remove<T>()
            where T : IAsyncDisposable;

        public IJSFacadeDictionaryConfiguration Clear();
    }
}
