// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Dynamic.Annotations;

namespace Teronis.Microsoft.JSInterop.Dynamic.DynamicObjects
{
    public interface IMisuedCancellableAnnotatedDynamicObject : IJSObjectReferenceFacade
    {
        ValueTask ProvoceTooManyCancellableAnnotatedParameterException([Cancellable] CancellationToken cancellationToken, [Cancellable] TimeSpan timeout);
    }
}
