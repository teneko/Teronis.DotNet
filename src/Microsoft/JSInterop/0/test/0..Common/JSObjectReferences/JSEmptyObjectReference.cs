using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.JSObjectReferences
{
    public class JSEmptyObjectReference : IJSObjectReference
    {
        public bool IsDisposed { get; private set; }

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            new ValueTask<TValue>();

        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            new ValueTask<TValue>();

        public virtual ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return new ValueTask();
        }
    }
}
