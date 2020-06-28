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
            => IDebuggerDisplayTools.GetDebuggerDisplay(obj);
    }
}
