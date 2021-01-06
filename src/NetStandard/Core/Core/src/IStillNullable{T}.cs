using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public interface IStillNullable<out T>
        where T : notnull
    {
        [MaybeNull]
        T Value { get; }
        bool HasValue { get; }
    }
}
