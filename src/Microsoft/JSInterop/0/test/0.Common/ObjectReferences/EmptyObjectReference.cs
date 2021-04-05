// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.ObjectReferences
{
    public class EmptyObjectReference : IJSObjectReference
    {
        public bool IsDisposed { get; private set; }
        public object? Tag { get; }

        public EmptyObjectReference() { }

        public EmptyObjectReference(object? tag) =>
            Tag = tag;

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
