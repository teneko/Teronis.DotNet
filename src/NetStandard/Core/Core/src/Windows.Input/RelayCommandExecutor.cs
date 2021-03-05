using System.Diagnostics.CodeAnalysis;

namespace Teronis.Windows.Input
{
    public delegate void RelayCommandExecutor<in T>([AllowNull]T obj);
}
