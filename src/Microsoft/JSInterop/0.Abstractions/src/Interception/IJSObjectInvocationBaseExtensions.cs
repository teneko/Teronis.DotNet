// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Microsoft.JSInterop.Interception
{
    public static class IJSObjectInvocationBaseExtensions
    {
        /// <summary>
        /// Ensures that the function invocation is similiar to a getter invocation.
        /// This is not the case then the number of arguments is greater than zero.
        /// </summary>
        /// <param name="invocation"></param>
        /// <exception cref="ArgumentException">Thrown when not a getter.</exception>
        public static void EnsureMimickingGetter(this IJSObjectInvocationBase invocation) {
            if (invocation.Arguments.Length > 0) {
                throw new ArgumentException("When you mimic a getter you cannot specify arguments.");
            }
        }
    }
}
