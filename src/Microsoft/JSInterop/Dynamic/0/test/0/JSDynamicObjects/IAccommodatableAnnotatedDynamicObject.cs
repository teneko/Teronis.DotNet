// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface IAccommodatableAnnotatedDynamicObject : IJSObjectReferenceFacade
    {
        ValueTask<object?[]> GetJavaScriptArguments(object? ballast, [Cancellable] CancellationToken cancellationToken, [Accommodatable] params object?[] jsArguments);
    }
}
