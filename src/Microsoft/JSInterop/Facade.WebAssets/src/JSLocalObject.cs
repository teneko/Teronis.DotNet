using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.WebAssets
{
    public readonly struct JSLocalObject : IAsyncDisposable, IJSLocalObject
    {
        public readonly IJSObjectReference JSObjectReference { get; }

        private readonly IJSObjectInterop objectInterop;

        public JSLocalObject(IJSObjectReference objectReference, IJSObjectInterop objectInterop)
        {
            JSObjectReference = objectReference ?? throw new ArgumentNullException(nameof(objectReference));
            this.objectInterop = objectInterop ?? throw new ArgumentNullException(nameof(objectInterop));
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            JSObjectReference.InvokeAsync<TValue>(identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            JSObjectReference.InvokeAsync<TValue>(identifier, cancellationToken, args);

        public ValueTask<IJSLocalObject> CreateObjectAsync(string objectName) =>
            objectInterop.CreateObjectAsync(this, objectName);

        public ValueTask DisposeAsync() =>
            JSObjectReference.DisposeAsync();
    }
}
