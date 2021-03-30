// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.ObjectReferences
{
    public class CancellableObjectReference : EmptyObjectReference
    {
        public override ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            if (cancellationToken.IsCancellationRequested) {
                throw new ObjectReferenceInvocationCanceledException(identifier, args);
            }

            return base.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }
    }
}
