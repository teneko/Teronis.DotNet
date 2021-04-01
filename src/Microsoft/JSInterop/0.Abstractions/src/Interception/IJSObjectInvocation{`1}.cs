// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Reflection;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public interface IJSObjectInvocation<TValue> : IJSObjectInvocationBase<ValueTask<TValue>>
    {
        Type TaskArgumentType { get; }
        /// <summary>
        /// The cacheable attributes of <see cref="TaskArgumentType"/>.
        /// </summary>
        ICustomAttributes TaskArgumentTypeAttributes { get; }
        IDefinition Definition { get; }

        /// <summary>
        /// Gets the value that that won't be considered as determined. In other words, the value won't be cached.
        /// </summary>
        /// <typeparam name="TNonDeterminingValue">Type of value.</typeparam>
        /// <returns>The promise for the non-determining value.</returns>
        ValueTask<TNonDeterminingValue> GetNonDeterminingResult<TNonDeterminingValue>();
        IJSObjectInvocation<TValue> Clone();
    }
}
