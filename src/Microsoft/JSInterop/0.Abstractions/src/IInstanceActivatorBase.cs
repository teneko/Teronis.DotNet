using System;

namespace Teronis.Microsoft.JSInterop
{
    public class IInstanceActivatorBase<T> : IInstanceActivator<T>
        where T : IAsyncDisposable
    {
        public event InstanceActivatedDelegate<T>? InstanceActivated;

        protected void DispatchInstanceActicated(T activatedInstance) =>
            InstanceActivated?.Invoke(activatedInstance);
    }
}
