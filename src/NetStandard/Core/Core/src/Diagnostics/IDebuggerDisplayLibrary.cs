using System.Diagnostics;

namespace Teronis.Diagnostics
{
    public static class IDebuggerDisplayLibrary
    {
        /// <summary>
        /// Represents the call to <see cref="IDebuggerDisplay.DebuggerDisplay"/> as string. It is full-namespace qualified and has "this" as first parameter.
        /// Useful when you need to fill in a this-referenced method to <see cref="IDebuggerDisplay.DebuggerDisplay"/> in <see cref="DebuggerDisplayAttribute"/>.
        /// </summary>
        public const string FullGetDebuggerDisplayMethodPathWithParameterizedThis = "{" + nameof(Teronis) + "." + nameof(Diagnostics) + "." + nameof(IDebuggerDisplayUtils) + "." + nameof(IDebuggerDisplayUtils.GetDebuggerDisplay) + "(this)}";
    }
}
