// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Diagnostics;

namespace Teronis.Extensions
{
    public static class IDebuggerDisplayExtensions
    {
        /// <summary>
        /// Looks for <see cref="IDebuggerDisplay"/> interface implementation. 
        /// If implemented, <see cref="IDebuggerDisplay.DebuggerDisplay"/> is 
        /// returned, otherwise <see cref="object.ToString"/>.
        /// </summary>
        public static string? GetDebuggerDisplay(this object? obj)
            => IDebuggerDisplayUtils.GetDebuggerDisplay(obj);
    }
}
