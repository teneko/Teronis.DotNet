using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{   
    public interface IJSCustomFacadeDictionary : IReadOnlyDictionary<Type, JSCustomFacadeFactoryDelegate<IAsyncDisposable>?>
    { }
}
