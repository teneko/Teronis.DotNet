using System.Diagnostics.CodeAnalysis;

namespace Teronis.Tools
{
    [return: MaybeNull]
    public delegate V GetInputDelegate<I, V>([AllowNull] I inValue);
}
