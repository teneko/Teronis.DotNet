using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{   
    public interface IJSFacadeDictionary : IReadOnlyDictionary<Type, JSFacadeCreatorDelegate<IAsyncDisposable>?>
    { }
}
