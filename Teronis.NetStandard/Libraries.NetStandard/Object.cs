using Teronis.Diagnostics;
using System.Diagnostics;
using Teronis.Extensions.NetStandard;

namespace Teronis.Libraries.NetStandard
{
    public static class ObjectLibrary
    {
        /// <summary>
        /// Represents the call to <see cref="IDebuggerDisplay.DebuggerDisplay"/> as string. It is full-namespace qualified and has "this" as first parameter.
        /// Useful when you need to fill in a this-referenced method to <see cref="IDebuggerDisplay.DebuggerDisplay"/> in <see cref="DebuggerDisplayAttribute"/>.
        /// </summary>
        public const string FullToDebugStringMethodPathWithParameterizedThis = "{" + nameof(Teronis) + "." + nameof(Extensions) + "." + nameof(NetStandard) + "." + nameof(ObjectExtensions) + "." + nameof(ObjectExtensions.ToDebugString) + "(this)}";
    }
}
