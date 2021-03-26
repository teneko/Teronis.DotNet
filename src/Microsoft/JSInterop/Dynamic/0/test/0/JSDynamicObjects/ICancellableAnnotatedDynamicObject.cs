// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.JSDynamicObjects
{
    public interface ICancellableAnnotatedDynamicObject : IJSObjectReferenceFacade
    {
        ValueTask CancelViaCancellationToken(string cancellationReadon, [Cancellable] CancellationToken cancellationToken, object? ballast);
        ValueTask CancelViaTimeSpan(string cancellationReadon, [Cancellable] TimeSpan timeout, object? ballast);
    }
}
