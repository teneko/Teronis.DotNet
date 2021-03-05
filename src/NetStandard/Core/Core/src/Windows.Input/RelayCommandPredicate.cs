using System.Diagnostics.CodeAnalysis;

namespace Teronis.Windows.Input
{
    public delegate bool RelayCommandPredicate<in T>([AllowNull]T obj);
}
