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
        /// <summary>
        /// The identifier (property/function) to be passed through.
        /// </summary>
        string Identifier { get; }
        /// <summary>
        /// The arguments to be passed through.
        /// </summary>
        object?[] Arguments { get; }
        /// <summary>
        /// The cancellation token to be passed through.
        /// </summary>
        CancellationToken? CancellationToken { get; }
        /// <summary>
        /// Attributes that defines custom behaviour.
        /// </summary>
        ICustomAttributes InvocationAttributes { get; }
        IMemberDefinition InvocationDefinition { get; }

        /// <summary>
        /// Indicates whether the value task is set and can be awaited.
        /// </summary>
        bool HasDeterminedResult { get; }
        bool HasAlternativeObjectReference { get; }
        /// <summary>
        /// The alternative JavaSript object reference or the original JavaScript
        /// object reference if alternative JavaSript object reference is null.
        /// </summary>
        IJSObjectReference ObjectReference { get; set; }
        /// <summary>
        /// The JavaScript object reference that got initially passed.
        /// </summary>
        IJSObjectReference OriginalObjectReference { get; }

        /// <summary>
        /// Gets the determined result.
        /// </summary>
        /// <returns>Expected to be <see cref="ValueTask"/> or <see cref="ValueTask{TResult}"/>.</returns>
        object GetDeterminedResult();
    }
}
