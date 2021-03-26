// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Diagnostics
{
    public static class IDebuggerDisplayUtils
    {
        /// <summary>
        /// Looks for <see cref="IDebuggerDisplay"/> interface implementation. 
        /// If implemented, <see cref="IDebuggerDisplay.DebuggerDisplay"/> is 
        /// returned, otherwise <see cref="object.ToString"/>.
        /// </summary>
        public static string? GetDebuggerDisplay(object? obj)
        {
            if (obj is null) {
                return "No debugger information provided.";
            }

            if (obj is IDebuggerDisplay debuggerDisplay) {
                return debuggerDisplay.DebuggerDisplay;
            }

            return obj.ToString();
        }
    }
}
