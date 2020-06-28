using System;

namespace Teronis.Diagnostics
{
    public static class IDebuggerDisplayTools
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
