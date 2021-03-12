using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facade
{   
    public interface IJSFacadeDictionary : IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>
    { }
}
