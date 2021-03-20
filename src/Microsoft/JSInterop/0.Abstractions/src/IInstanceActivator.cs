using System;

namespace Teronis.Microsoft.JSInterop
{
    public interface IInstanceActivator<out T>
        where T : IAsyncDisposable
    {
        event InstanceActivatedDelegate<T>? InstanceActivated;
    }
}
