using System.Reflection;

namespace Teronis.Reflection
{
    public interface ITypeInfoFilter
    {
        bool IsAllowed(TypeInfo? typeInfo);
    }
}
