using System.Diagnostics.CodeAnalysis;

namespace Teronis
{
    public delegate void EventHandler<in TSender, in TArgs>([AllowNull]TSender sender, TArgs args);
}
