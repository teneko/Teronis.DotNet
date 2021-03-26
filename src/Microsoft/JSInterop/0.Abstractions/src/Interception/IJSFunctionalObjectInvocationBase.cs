// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using Microsoft.JSInterop;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSFunctionalObjectInvocationBase
    {
        bool IsInterceptionStopped { get; }

        object?[] Arguments { get; }
        CancellationToken? CancellationToken { get; }
        string Identifier { get; }
        bool HasAlternativeResult { get; }
        bool HasAlternativeJSObjectReference { get; }
        IJSObjectReference JSObjectReferenceResulting { get; set; }
        IJSObjectReference JSObjectReferencePassed { get; }

        void StopInterception();
    }
}
