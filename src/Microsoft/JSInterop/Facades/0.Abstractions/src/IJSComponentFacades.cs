using System;
using System.Collections.Generic;

namespace Teronis.Microsoft.JSInterop.Facades
{
    public interface IJSComponentFacades : IAsyncDisposable, IReadOnlyList<IAsyncDisposable>, IJSFacadeResolver
    { }
}
