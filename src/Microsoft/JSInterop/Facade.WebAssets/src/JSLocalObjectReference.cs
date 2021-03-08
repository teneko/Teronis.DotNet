using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Facade.WebAssets
{
    public readonly struct JSLocalObjectReference : IAsyncDisposable, IJSLocalObjectReference
    {
        private readonly IJSObjectReference objectReference;
        private readonly IJSObjectInterop objectInterop;

        public JSLocalObjectReference(IJSObjectReference objectReference, IJSObjectInterop objectInterop) {
            this.objectReference = objectReference ?? throw new ArgumentNullException(nameof(objectReference));
            this.objectInterop = objectInterop ?? throw new ArgumentNullException(nameof(objectInterop));
        }

        public ValueTask DisposeAsync() =>
            objectReference.DisposeAsync();

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            objectReference.InvokeAsync<TValue>(identifier, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            objectReference.InvokeAsync<TValue>(identifier, cancellationToken, args);

        public async ValueTask<IJSLocalObjectReference> CreateObjectReferenceAsync(string objectName) =>
            new JSLocalObjectReference(await objectInterop.CreateObjectReferenceAsync(objectReference, objectName), objectInterop);
    }
}
