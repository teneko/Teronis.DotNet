using System.Diagnostics.CodeAnalysis;

namespace Teronis.Tools
{
    [return: MaybeNull]
    public delegate O ReplaceValueDelegate<I, O>([AllowNull] I value);
}
