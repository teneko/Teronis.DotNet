using System;

namespace Teronis.Microsoft.JSInterop
{
    public delegate void InstanceActivatedDelegate<in T>(T activatedInstance)
        where T : IAsyncDisposable;
}
