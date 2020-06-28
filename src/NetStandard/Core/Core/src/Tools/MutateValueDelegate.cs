using System.Diagnostics.CodeAnalysis;

namespace Teronis.Tools
{
    public delegate void MutateValue<V>([AllowNull] V value);
}
