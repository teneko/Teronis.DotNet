using System.Diagnostics.CodeAnalysis;

namespace Teronis.Collections.Generic
{
    public interface INullableKey<out KeyType>
        where KeyType : notnull
    {
        bool IsNull { get; }
        [MaybeNull]
        KeyType Key { get; }
    }
}
