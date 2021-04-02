// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class IJSObjectInvocationGenericExtensions
    {
        public static void SetAlternativeResult<TValue>(this IJSObjectInvocation<TValue> invocation, TValue value) =>
            invocation.SetAlternativeResult(new ValueTask<TValue>(value));

        public static void SetAlternativeResult<TValue>(this IJSObjectInvocation<TValue> invocation, Task<TValue> valuePromise) =>
            invocation.SetAlternativeResult(new ValueTask<TValue>(valuePromise));
    }
}
