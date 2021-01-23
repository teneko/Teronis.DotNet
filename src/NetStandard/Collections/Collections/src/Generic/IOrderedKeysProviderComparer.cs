using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    /// <summary>
    /// Compares two instances of <see cref="IOrderedKeysProvider"/>.
    /// An instance is smaller if it the only null instance or its path is
    /// smaller than the path of the other instance.
    /// </summary>
    public class IOrderedKeysProviderComparer : Comparer<IOrderedKeysProvider>
    {
        public override int Compare([AllowNull] IOrderedKeysProvider x, [AllowNull] IOrderedKeysProvider y)
        {
            var xDependencyPath = x?.ToDependencyPath();
            var yDependencyPath = y?.ToDependencyPath();
            return string.Compare(xDependencyPath, yDependencyPath, System.StringComparison.Ordinal);
        }
    }
}
