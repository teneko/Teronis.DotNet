// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSObjectInvocationBase
    {
        bool IsInterceptionStopped { get; }

        object?[] JavaScriptArguments { get; }
        CancellationToken? JavaScriptCancellationToken { get; }
        string JavaScriptIdentifier { get; }
        ICustomAttributes MemberAttributes { get; }

        /// <summary>
        /// Indicates whether the value task is set and can be awaited.
        /// </summary>
        bool HasDeterminedResult { get; }
        bool HasAlternativeJSObjectReference { get; }
        /// <summary>
        /// The alternative JavaSript object reference or the original JavaScript
        /// object reference if alternative JavaSript object reference is null.
        /// </summary>
        IJSObjectReference JSObjectReferenceResulting { get; set; }
        /// <summary>
        /// The JavaScript object reference that got initially passed.
        /// </summary>
        IJSObjectReference JSObjectReferencePassed { get; }

        object GetDeterminedResult();
        void StopInterception();
    }
}
