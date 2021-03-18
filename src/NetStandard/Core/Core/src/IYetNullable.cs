using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public interface IYetNullable<out T>
    {
        [MaybeNull]
        T Value { get; }
        bool IsNull { get; }
        bool IsNotNull { get; }
    }
}
